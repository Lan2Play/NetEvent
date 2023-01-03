using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;

namespace NetEvent.Client.Pages
{
    public partial class LegalNotice
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        #endregion

        private bool _Loading = true;
        private string _LegalNotice = string.Empty;
        private string _PrivacyPolicy = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _LegalNotice = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.LegalNotice, cts.Token))?.Value ?? string.Empty;
            _PrivacyPolicy = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.PrivacyPolicy, cts.Token))?.Value ?? string.Empty;

            _Loading = false;
        }
    }
}
