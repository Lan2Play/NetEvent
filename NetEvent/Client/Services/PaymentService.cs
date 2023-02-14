using System.Net.Http;
using System.Threading;
using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using NetEvent.Shared.Dto;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto.Event;
using System.Collections.Generic;

namespace NetEvent.Client.Services
{
    public class PaymentService : IPaymentService
    {
        private const string _CartKey = "cart";

        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<PaymentService> _Logger;
        private readonly ILocalStorageService _LocalStorage;

        public PaymentService(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory, ILogger<PaymentService> logger)
        {
            _LocalStorage = localStorage;
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<CartDto> LoadCart(string cartId)
        {
            if (await _LocalStorage.ContainKeyAsync(cartId).ConfigureAwait(false))
            {
                return await _LocalStorage.GetItemAsync<CartDto>(_CartKey);
            }

            var cart = new CartDto();
            await _LocalStorage.SetItemAsync(_CartKey, cart);
            return cart;
        }

        public async Task<ServiceResult<CheckoutSessionDto?>> BuyTicketAsync(long id, int amount, CancellationToken cancellationToken)
        {
            try
            {
                var ticketCart = new CartDto { CartEntries = new[] { new CartEntryDto { TicketId = id, Amount = amount } } };

                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/payment/buy", ticketCart, cancellationToken);
                response.EnsureSuccessStatusCode();

                var sessionResponse = await response.Content.ReadFromJsonAsync<CheckoutSessionDto>().ConfigureAwait(false);

                return ServiceResult<CheckoutSessionDto?>.Success(sessionResponse, "EventService.AddAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to create eventTicketType in backend.");
            }

            return ServiceResult<CheckoutSessionDto?>.Error("EventService.AddAsync.Error");
        }

        public async Task<ServiceResult<IReadOnlyCollection<PaymentMethodDto>?>> LoadPaymentMethodsAsync(long amount, CurrencyDto currency, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.GetFromJsonAsync<IReadOnlyCollection<PaymentMethodDto>>($"api/payment/paymentmethods/{amount}/{currency}", cancellationToken);

                return ServiceResult<IReadOnlyCollection<PaymentMethodDto>?>.Success(response);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to create eventTicketType in backend.");
            }

            return ServiceResult<IReadOnlyCollection<PaymentMethodDto>?>.Error("PaymentService.LoadPaymentMethodsAsync.Error");
        }
    }
}
