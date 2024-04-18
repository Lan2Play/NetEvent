using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages.Checkout
{
    public partial class CheckoutTicket
    {
        #region Injects

        [Inject]
        private IEventService EventService { get; set; } = default!;

        [Inject]
        private IPaymentService PaymentService { get; set; } = default!;

        [Inject]
        private NavigationService NavigationService { get; set; } = default!;

        [Inject]
        private ISystemSettingsDataService SettingsService { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Inject]
        private IJSRuntime JsRuntime { get; set; } = default!;

        #endregion

        #region Parameters

        [Parameter]
        public long TicketType { get; set; }

        [Parameter]
        public int? Amount { get; set; }

        #endregion

        private EventTicketTypeDto? EventTicketType;
        private CheckoutSessionDto? CheckoutSession;
        private IReadOnlyCollection<PaymentMethodDto>? PaymentMethods;
        private PurchaseDto _Purchase;
        private Severity _ResultSeverity;
        private LocalizedString? _Result;
        private LocalizedString? _ResultRefused;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            Amount ??= 1;

            EventTicketType = await EventService.GetEventTicketTypeAsync(TicketType, cts.Token).ConfigureAwait(false);
            if (EventTicketType == null)
            {
                NavigationService.NavigateBack();
            }
        }

        private async Task BuyTicketAsync()
        {
            if (EventTicketType?.Id == null || Amount == null)
            {
                return;
            }

            var cts = new CancellationTokenSource();
            var result = await PaymentService.BuyTicketAsync(EventTicketType.Id.Value, Amount.Value, cts.Token).ConfigureAwait(false);
            if (result.Successful && result.ResultData != null)
            {
                _Purchase = result.ResultData;
                var paymentMethods = await PaymentService.LoadPaymentMethodsAsync(EventTicketType.Price, EventTicketType.Currency, cts.Token).ConfigureAwait(false);
                if (result.Successful)
                {
                    PaymentMethods = paymentMethods.ResultData;
                    var clientKey = await SettingsService.GetSystemSettingAsync(SystemSettingGroup.PaymentData, SystemSettings.PaymentData.AdyenClientKey, cts.Token).ConfigureAwait(false);
                    var paymentMethod = System.Text.Json.JsonSerializer.Serialize(new { paymentMethods = PaymentMethods }, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });
                    await JsRuntime.InvokeVoidAsync("checkout.startPaymentAsync", clientKey?.Value, paymentMethod, DotNetObjectReference.Create(this)).ConfigureAwait(false);
                }
            }
        }

        [JSInvokable]
        public async Task<string?> MakePayment(JsonElement data)
        {
            Console.WriteLine(_Purchase.Id);
            var dataJson = data.GetRawText();
            var paymentResponse = await PaymentService.MakePaymentAsync(_Purchase.Id, dataJson, CancellationToken.None).ConfigureAwait(false);

            return paymentResponse?.ResultData?.PaymentResponseJson;
        }

        [JSInvokable]
        public Task<object> MakeDetailsCall(object data)
        {
            return Task.FromResult<object>(null);
        }

        private static class PurchaseResultCode
        {
            public const int AuthenticationFinished = 0;
            public const int AuthenticationNotRequired = 1;
            public const int Authorised = 2;
            public const int Received = 9;

            public const int Cancelled = 3;

            public const int Error = 5;
            public const int ChallengeShopper = 4;
            public const int IdentifyShopper = 6;
            public const int Pending = 7;
            public const int PresentToShopper = 8;
            public const int RedirectShopper = 10;

            public const int Refused = 11;
        }

        [JSInvokable]
        public void ShowResult(int resultCode, string? refusedCode)
        {
            // https://docs.adyen.com/online-payments/payment-result-codes
            _ResultSeverity = resultCode switch
            {
                PurchaseResultCode.AuthenticationFinished or PurchaseResultCode.AuthenticationNotRequired or PurchaseResultCode.Authorised or PurchaseResultCode.Received => Severity.Success,
                PurchaseResultCode.Error => Severity.Error,
                PurchaseResultCode.Cancelled or PurchaseResultCode.ChallengeShopper or PurchaseResultCode.IdentifyShopper or PurchaseResultCode.Pending or PurchaseResultCode.PresentToShopper or PurchaseResultCode.RedirectShopper => Severity.Info,
                PurchaseResultCode.Refused => Severity.Warning,
                _ => Severity.Normal,
            };
            _Result = Localizer[$"CheckoutTicket.Result.{resultCode}"];
            if (refusedCode != null)
            {
                // https://docs.adyen.com/development-resources/refusal-reasons
                _ResultRefused = Localizer[$"CheckoutTicket.ResultRefused.{refusedCode}"];
            }

            StateHasChanged();
        }
    }
}
