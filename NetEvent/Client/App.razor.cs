using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client
{
    public partial class App
    {
        private const string _StyleUrl = "css/netevent.css";
        private string _Style = _StyleUrl;

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        protected override Task OnInitializedAsync()
        {
            _SystemSettingsDataService.SubscribeSystemSettingGroupChange(SystemSettingGroup.StyleData, StyleSettingGroupChanged);
            return base.OnInitializedAsync();
        }

        private void StyleSettingGroupChanged(SystemSettingValueDto value)
        {
            _Style = $"{_StyleUrl}?r={DateTime.UtcNow.Ticks}";
            StateHasChanged();
        }
    }
}
