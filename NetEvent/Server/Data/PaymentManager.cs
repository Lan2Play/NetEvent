using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Adyen;
using Adyen.Model.Checkout;
using Adyen.Service.Checkout;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Helpers;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;
using Newtonsoft.Json;

namespace NetEvent.Server.Data
{
    public class PaymentManager : IPaymentManager
    {
        private readonly ApplicationDbContext _DbContext;

        private readonly NetEventUserManager _UserManager;

        private readonly ILogger<PaymentManager> _Logger;

        public PaymentManager(ApplicationDbContext dbContext, NetEventUserManager userManager, ILogger<PaymentManager> logger)
        {
            _DbContext = dbContext;
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<PaymentMethodsResponse> GetPaymentMethodsAsync(long amount, CurrencyDto currency)
        {
            var merchantAccount = await _DbContext.SystemSettingValues.FindAsync(SystemSettings.PaymentData.AdyenMerchantAccount).ConfigureAwait(false);
            var apiKey = await _DbContext.SystemSettingValues.FindAsync(SystemSettings.PaymentData.AdyenApiKey).ConfigureAwait(false);

            if (string.IsNullOrEmpty(merchantAccount?.SerializedValue))
            {
                // TODO Error
                return null;
            }

            if (string.IsNullOrEmpty(apiKey?.SerializedValue))
            {
                // TODO Error
                return null;
            }

            var config = new Config
            {
                XApiKey = apiKey.SerializedValue,
                Environment = Adyen.Model.Environment.Test
            };
            var client = new Adyen.Client(config);
            var checkout = new PaymentsService(client);

            var paymentMethodsRequest = new PaymentMethodsRequest(merchantAccount: merchantAccount.SerializedValue)
            {
                CountryCode = new RegionInfo(CultureInfo.CurrentUICulture.LCID).TwoLetterISORegionName,
                ShopperLocale = CultureInfo.CurrentUICulture.Name,
                Amount = new Amount(currency.To3DigitIso(), amount),
                Channel = PaymentMethodsRequest.ChannelEnum.Web
            };

            var paymentMethodsResponse = checkout.PaymentMethods(paymentMethodsRequest);
            return paymentMethodsResponse;
        }

        public async Task<Purchase> PurchaseAsync(CartDto cart, ClaimsPrincipal claimsPrincipal)
        {
            var merchantAccount = await _DbContext.SystemSettingValues.FindAsync(SystemSettings.PaymentData.AdyenMerchantAccount).ConfigureAwait(false);
            var apiKey = await _DbContext.SystemSettingValues.FindAsync(SystemSettings.PaymentData.AdyenApiKey).ConfigureAwait(false);

            if (string.IsNullOrEmpty(merchantAccount?.SerializedValue))
            {
                // TODO Error
                return null;
            }

            if (string.IsNullOrEmpty(apiKey?.SerializedValue))
            {
                // TODO Error
                return null;
            }

            var purchase = await CreatePurchaseAsync(cart, claimsPrincipal).ConfigureAwait(false);

            if (purchase == null)
            {
                // TODO Error
                return null;
            }

            var currencyGroup = purchase.TicketPurchases.GroupBy(x => x.Currency);
            if (currencyGroup.Count() > 1)
            {
                // TODO Error
                return null;
            }

            await _DbContext.Purchases.AddAsync(purchase).ConfigureAwait(false);
            await _DbContext.SaveChangesAsync().ConfigureAwait(false);

            return purchase;
        }

        public async Task<PaymentResponse?> SubmitDropInEventDataAsync(ClaimsPrincipal claimsPrincipal, string purchaseId, string paymentMethodData)
        {
            var merchantAccount = await _DbContext.SystemSettingValues.FindAsync(SystemSettings.PaymentData.AdyenMerchantAccount).ConfigureAwait(false);
            var apiKey = await _DbContext.SystemSettingValues.FindAsync(SystemSettings.PaymentData.AdyenApiKey).ConfigureAwait(false);
            var purchase = await _DbContext.Purchases.Include(p => p.TicketPurchases).FirstOrDefaultAsync(p => p.Id == purchaseId).ConfigureAwait(false);

            if (string.IsNullOrEmpty(merchantAccount?.SerializedValue))
            {
                // TODO Error
                return null;
            }

            if (string.IsNullOrEmpty(apiKey?.SerializedValue))
            {
                // TODO Error
                return null;
            }


            if (purchase == null)
            {
                // TODO Error
                return null;
            }

            var currencyGroup = purchase.TicketPurchases.GroupBy(x => x.Currency);
            if (currencyGroup.Count() > 1)
            {
                // TODO Error
                return null;
            }

            var paymentRequest = JsonConvert.DeserializeObject<PaymentRequest>(paymentMethodData);

            if (paymentRequest == null)
            {
                // TODO Error
                return null;
            }

            paymentRequest.MerchantAccount = merchantAccount.SerializedValue;
            paymentRequest.Reference = purchase.Id;
            paymentRequest.ReturnUrl = "https://your-company.com/checkout?shopperOrder=12xy..";
            paymentRequest.Amount = new Amount(currencyGroup.First().Key.ToCurrencyDto().To3DigitIso(), purchase.Price);
            paymentRequest.ShopperInteraction = PaymentRequest.ShopperInteractionEnum.Ecommerce;

            //var paymentRequest = new PaymentRequest
            //{
            //    PaymentMethod = System.Text.Json.JsonSerializer.Deserialize<DefaultPaymentMethodDetails>(paymentMethodData),
            //    MerchantAccount = merchantAccount.SerializedValue,
            //    Reference = purchase.Id,
            //    ReturnUrl = "https://your-company.com/checkout?shopperOrder=12xy..", // TODO Checkout Seite mit Polling/Events/...
            //    Amount = new Amount(currencyGroup.First().Key.ToCurrencyDto().To3DigitIso(), purchase.Price),
            //    //CountryCode = new RegionInfo(CultureInfo.CurrentUICulture.LCID).TwoLetterISORegionName,
            //};

            //Create the http client
            var config = new Config
            {
                XApiKey = apiKey.SerializedValue,
                Environment = Adyen.Model.Environment.Test
            };
            var client = new Adyen.Client(config);
            var checkout = new PaymentsService(client);
            var paymentResult = await checkout.PaymentsAsync(paymentRequest).ConfigureAwait(false);
            return paymentResult;
        }

        #region Helpers

        private async Task<Purchase?> CreatePurchaseAsync(CartDto cart, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.Id();
            var user = await _UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                const string errorMessage = "User not found.";
                _Logger.LogError(errorMessage);
                return null;
            }

            string purchaseId;
            do
            {
                purchaseId = Guid.NewGuid().ToString();
            }
            while ((await _DbContext.Purchases.FindAsync(purchaseId)) != null);

            var purchase = new Purchase
            {
                Id = purchaseId,
                PurchaseTime = DateTime.UtcNow,
                User = user,
                UserId = user.Id,
            };

            await CreateTicketPurchasesAsync(cart, purchase).ConfigureAwait(false);

            return purchase;
        }

        private async Task CreateTicketPurchasesAsync(CartDto cart, Purchase purchase)
        {
            var maxTicketPurchaseId = await _DbContext.TicketPurchases.MaxAsync(x => x.Id).ConfigureAwait(false);
            var ticketPurchases = new List<TicketPurchase>();
            foreach (var cartTicket in cart.CartEntries.Where(e => e.TicketId != null))
            {
                var ticketType = await _DbContext.Tickets.FindAsync(cartTicket.TicketId).ConfigureAwait(false);
                if (ticketType == null)
                {
                    throw new NotSupportedException($"TicketId {cartTicket.TicketId} not found!");
                }

                ticketPurchases.AddRange(Enumerable.Repeat<object?>(null, cartTicket.Amount).Select(_ => new TicketPurchase
                {
                    Id = ++maxTicketPurchaseId,
                    Currency = ticketType.Currency,
                    Price = ticketType.Price,
                    Purchase = purchase,
                    PurchaseId = purchase.Id,
                    Ticket = ticketType,
                    TicketId = ticketType.Id
                }));
            }

            if (ticketPurchases.Any())
            {
                purchase.TicketPurchases = ticketPurchases;
            }
        }

        #endregion
    }
}
