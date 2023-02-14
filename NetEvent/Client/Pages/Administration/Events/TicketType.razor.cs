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
using NetEvent.Shared.Dto.Event;
using NetEvent.Client.Services;
using System.Globalization;
using System.Threading;
using NetEvent.Shared.Validators;
using Microsoft.Extensions.Localization;
using NetEvent.Client.Extensions;

namespace NetEvent.Client.Pages.Administration.Events
{
    public partial class TicketType
    {
        private const int _DayOffset = 2;

        #region Injects

        [Inject]
        private IEventService EventService { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Inject]
        private NavigationService NavigationService { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        #endregion

        [Parameter]
        public string? EventId { get; set; }

        [Parameter]
        public string? TicketTypeId { get; set; }

        private readonly EventTicketTypeModelFluentValidator _EventTicketTypeValidator = new();
        private bool _Loading = true;
        private EventTicketTypeDto _EventTicketType = default!;
        private MudForm? _Form = default!;
        private long _EventId;
        private CurrencyConverter? CurrencyConverter;

        public double Price { get => _EventTicketType.Currency.ToCurrencyValue(_EventTicketType.Price); set => _EventTicketType.Price = _EventTicketType.Currency.ToCurrencyBaseValue(value); }

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            if (!long.TryParse(EventId, CultureInfo.InvariantCulture, out _EventId))
            {
                Snackbar.Add("No event defined!", Severity.Error);
            }

            if (long.TryParse(TicketTypeId, CultureInfo.InvariantCulture, out var id))
            {
                _EventTicketType = (await EventService.GetEventTicketTypeAsync(id, cts.Token).ConfigureAwait(false)) ?? new EventTicketTypeDto();
            }
            else
            {
                _EventTicketType = new EventTicketTypeDto
                {
                    SellStartDate = DateTime.Today.AddDays(_DayOffset - 1).AddHours(24 - DateTime.Today.Hour),
                    SellEndDate = DateTime.Today.AddDays(_DayOffset).AddHours(24 - DateTime.Today.Hour),
                };
            }

            CurrencyConverter = new CurrencyConverter(_EventTicketType.Currency);

            _Loading = false;
        }

        private async Task SaveEventTicketType()
        {
            if (_Form == null)
            {
                return;
            }

            await _Form.Validate();

            if (_Form.IsValid)
            {
                var cts = new CancellationTokenSource();
                ServiceResult result;
                if (_EventTicketType.Id >= 0)
                {
                    result = await EventService.UpdateEventTicketTypeAsync(_EventTicketType, cts.Token);
                }
                else
                {
                    result = await EventService.CreateEventTicketTypeAsync(_EventId, _EventTicketType, cts.Token);

                    if (result.Successful && _EventTicketType?.Id != null)
                    {
                        NavigationService.NavigateTo(UrlHelper.GetEventLink(_EventTicketType.Id, true));
                    }
                }

                if (!string.IsNullOrEmpty(result.MessageKey))
                {
                    Snackbar.Add(Localizer.GetString(result.MessageKey), result.Successful ? Severity.Success : Severity.Error);
                }
            }
        }
    }
}
