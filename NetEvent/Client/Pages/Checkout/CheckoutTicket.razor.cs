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

namespace NetEvent.Client.Pages.Checkout
{
    public partial class CheckoutTicket
    {
        #region Injects

        [Inject]
        private IEventService EventService { get; set; } = default!;

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

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            Amount ??= 1;

            EventTicketType = await EventService.GetEventTicketTypeAsync(TicketType, cts.Token).ConfigureAwait(false);
            // TODO Load PaymentMethods via new IPaymentService
        }

        private async Task BuyTicketAsync()
        {
            if (EventTicketType?.Id == null || Amount == null)
            {
                return;
            }

            var cts = new CancellationTokenSource();
            var result = await EventService.BuyTicketAsync(EventTicketType.Id.Value, Amount.Value, cts.Token).ConfigureAwait(false);
            if (result.Successful && result.ResultData != null)
            {
                var clientKey = await SettingsService.GetSystemSettingAsync(SystemSettingGroup.PaymentData, SystemSettings.PaymentData.AdyenClientKey, cts.Token).ConfigureAwait(false);

                CheckoutSession = result.ResultData;
                await JsRuntime.InvokeVoidAsync("checkout.startPaymentAsync", clientKey?.Value, CheckoutSession.Id, CheckoutSession.SessionData).ConfigureAwait(false);
            }
        }
    }
}
