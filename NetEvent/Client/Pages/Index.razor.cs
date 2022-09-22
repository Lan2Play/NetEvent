using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages
{
    public partial class Index
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        [Inject]
        private IEventService _EventService { get; set; } = default!;

        #endregion
        private static System.Timers.Timer? _Timer;

        private string? _OrganizationName;
        private string? _OrganizationAboutUs;

        private EventDto? _UpcomingEvent;
        private TimeSpan? _TimeLeft;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            _OrganizationName = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.OrganizationName, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            _OrganizationAboutUs = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.AboutUs, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            _UpcomingEvent = await _EventService.GetUpcomingEventAsync(cancellationTokenSource.Token);
            if (_UpcomingEvent != null)
            {
                _Timer = new System.Timers.Timer(1000);
                _Timer.Elapsed += (s, a) => UpdateTimeLeft();
                _Timer.Start();
                UpdateTimeLeft();
            }
        }

        private void UpdateTimeLeft()
        {
            if (_UpcomingEvent != null)
            {
                _TimeLeft = _UpcomingEvent.StartDate - System.DateTime.UtcNow;
                StateHasChanged();
            }
        }
    }
}
