using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Adyen.Model.Checkout;
using Adyen.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Helpers;
using NetEvent.Server.Models;
using NetEvent.Server.Modules;
using NetEvent.Shared;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

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

        public async Task<CreateCheckoutSessionResponse> PayAsync(CartDto cart, ClaimsPrincipal claimsPrincipal)
        {
            var paymentId = Guid.NewGuid();

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
                return null;
            }

            var currencyGroup = purchase.TicketPurchases.GroupBy(x => x.Currency);
            if (currencyGroup.Count() > 1)
            {
                return null;
            }

            var checkoutSessionRequest = new CreateCheckoutSessionRequest
            {
                MerchantAccount = merchantAccount.SerializedValue,
                Reference = paymentId.ToString(),
                ReturnUrl = "https://your-company.com/checkout?shopperOrder=12xy..", // TODO Checkout Seite mit Polling/Events/...
                Amount = new Amount(currencyGroup.First().Key.ToCurrencyDto().To3DigitIso(), purchase.Price),
                CountryCode = CultureInfo.CurrentCulture.ToString(),
            };
            var client = new Adyen.Client(apiKey.SerializedValue, Adyen.Model.Enum.Environment.Test);
            var checkout = new Checkout(client);
            return checkout.Sessions(checkoutSessionRequest);
        }

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

            var maxPurchaseId = await _DbContext.Purchases.MaxAsync(x => x.Id).ConfigureAwait(false);

            var purchase = new Purchase
            {
                Id = maxPurchaseId + 1,
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
    }
}
