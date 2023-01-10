using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages
{
    public partial class Index : IDisposable
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        [Inject]
        private IEventService EventService { get; set; } = default!;

        #endregion

        private System.Timers.Timer? _Timer;

        private string? _OrganizationName;
        private string? _OrganizationAboutUs;

        private EventDto? _UpcomingEvent;
        private TimeSpan? _TimeLeft;
        private bool _DisposedValue;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            _OrganizationName = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.OrganizationName, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            _OrganizationAboutUs = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.AboutUs, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            _UpcomingEvent = await EventService.GetUpcomingEventAsync(cancellationTokenSource.Token);
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
                _TimeLeft = _UpcomingEvent.StartDate - DateTime.UtcNow;
                StateHasChanged();
            }
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_DisposedValue)
            {
                if (disposing)
                {
                    _Timer?.Dispose();
                }

                _DisposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
