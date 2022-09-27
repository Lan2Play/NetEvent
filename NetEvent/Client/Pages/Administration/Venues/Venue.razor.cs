using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Extensions;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Event;
using NetEvent.Shared.Validators;

namespace NetEvent.Client.Pages.Administration.Venues
{
    public partial class Venue
    {
        #region Injects

        [Inject]
        private IVenueService _VenueService { get; set; } = default!;

        [Inject]
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        [Parameter]
        public string? Id { get; set; }

        private readonly VenueModelFluentValidator _VenueValidator = new();
        private bool _Loading = true;
        private MudForm? _Form = default!;
        private VenueDto _Venue = default!;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            if (long.TryParse(Id, out var id))
            {
                _Venue = await _VenueService.GetVenueAsync(id, cts.Token) ?? new VenueDto();
            }
            else
            {
                _Venue = new VenueDto();
            }

            _Loading = false;
        }

        private async Task SaveVenue()
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
                if (_Venue.Id >= 0)
                {
                    result = await _VenueService.UpdateVenueAsync(_Venue, cts.Token);
                }
                else
                {
                    result = await _VenueService.CreateVenueAsync(_Venue, cts.Token);
                    if (result.Successful && _Venue?.Id != null)
                    {
                        NavigationManager.NavigateTo(UrlHelper.GetVenueLink(_Venue.Id, true));
                    }
                }

                if (!string.IsNullOrEmpty(result.MessageKey))
                {
                    _Snackbar.Add(_Localizer.GetString(result.MessageKey), result.Successful ? Severity.Success : Severity.Error);
                }
            }
        }
    }
}
