using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Components.Administration
{
    public partial class SystemSettingControl
    {
        private string? _Style = null;

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        [Parameter]
        public SystemSetting SystemSetting { get; set; } = default!;

        [Parameter]
        public SystemSettingValueDto? Value { get; set; } = default!;

        public string? SettingValue
        {
            get => Value!.Value;
            set
            {
                Value!.Value = value;
                _ = OnSettingsValueChanged(value);
            }
        }

        private async Task OnSettingsValueChanged<T>(T? value)
        {
            if (value is null)
            {
                return;
            }

            _ = await _SystemSettingsDataService.UpdateSystemSetting(SystemSetting.SettingType, new SystemSettingValueDto(SystemSetting.Key, value.ToString() ?? string.Empty), CancellationToken.None);

            _Style = $"css/netevent.css?{DateTime.UtcNow.Ticks}";
        }
    }
}
