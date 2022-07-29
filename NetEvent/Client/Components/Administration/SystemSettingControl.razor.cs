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

        private async Task OnSettingsValueChanged(SystemSetting setting, object? value)
        {
            if (value is null)
            {
                return;
            }

            var result = await _SystemSettingsDataService.UpdateSystemSetting(setting.SettingType, new SystemSettingValueDto(setting.Key, value.ToString() ?? string.Empty), CancellationToken.None);
            if (result.Successful && Value != null)
            {
                Value.Value = value.ToString() ?? string.Empty;

            }
        }
    }
}
