using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MudBlazor;
using NetEvent.Client;
using NetEvent.Client.Shared;
using NetEvent.Client.Components;
using NetEvent.Client.Services;
using System.Threading;
using NetEvent.Shared.Dto.Event;
using Microsoft.Extensions.Localization;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Config;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;

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

        [JSInvokable]
        public void ShowResult(int resultCode, string? refusedCode)
        {
            // https://docs.adyen.com/online-payments/payment-result-codes
            switch (resultCode)
            {
                case 0 /* AuthenticationFinished */:
                case 1 /* AuthenticationNotRequired */:
                case 2 /* Authorised */:
                case 9 /* Received */:
                    _ResultSeverity = Severity.Success;
                    break;
                case 5 /* Error */:
                    _ResultSeverity = Severity.Error;
                    break;
                case 3 /* Cancelled */:
                case 4 /* ChallengeShopper */:
                case 6 /* IdentifyShopper */:
                case 7 /* Pending */:
                case 8 /* PresentToShopper */:
                case 10 /* RedirectShopper */:
                    _ResultSeverity = Severity.Info;
                    break;
                case 11 /* Refused */:
                    _ResultSeverity = Severity.Warning;



                    break;
                default:
                    _ResultSeverity = Severity.Normal;
                    break;
            }

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
