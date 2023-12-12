using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

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

        public async Task<ServiceResult<PurchaseDto?>> BuyTicketAsync(long id, int amount, CancellationToken cancellationToken)
        {
            try
            {
                var ticketCart = new CartDto { CartEntries = new[] { new CartEntryDto { TicketId = id, Amount = amount } } };

                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/payment/checkout/buy", ticketCart, cancellationToken);
                response.EnsureSuccessStatusCode();

                var sessionResponse = await response.Content.ReadFromJsonAsync<PurchaseDto>(cancellationToken: cancellationToken).ConfigureAwait(false);

                return ServiceResult<PurchaseDto?>.Success(sessionResponse, "EventService.AddAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to create eventTicketType in backend.");
            }

            return ServiceResult<PurchaseDto?>.Error("EventService.AddAsync.Error");
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

        public async Task<ServiceResult<PaymentResponseDto>> MakePaymentAsync(string purchaseId, string paymentDataJson, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/payment/checkout/{purchaseId}/payments", paymentDataJson, cancellationToken);
                response.EnsureSuccessStatusCode();
                var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponseDto>(cancellationToken: cancellationToken).ConfigureAwait(false);
                return ServiceResult<PaymentResponseDto>.Success(paymentResponse);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to create eventTicketType in backend.");
            }

            return ServiceResult<PaymentResponseDto>.Error("PaymentService.LoadPaymentMethodsAsync.Error");
        }
    }
}
