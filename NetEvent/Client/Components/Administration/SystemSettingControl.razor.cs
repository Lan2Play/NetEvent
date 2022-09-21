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
                _ = OnSettingsValueChanged(value);
            }
        }

        private async Task OnSettingsValueChanged<T>(T? value)
        {
            var stringValue = value?.ToString() ?? string.Empty;
            if (stringValue.Equals(SettingValue))
            {
                return;
            }

            _ = await _SystemSettingsDataService.UpdateSystemSetting(SystemSetting.SettingType, new SystemSettingValueDto(SystemSetting.Key, stringValue), CancellationToken.None);
            Value!.Value = stringValue;
        }
    }
}
