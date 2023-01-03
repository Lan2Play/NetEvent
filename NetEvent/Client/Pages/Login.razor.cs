using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages
{
    public partial class Login
    {
        [Inject]
        private NetEventAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Inject]
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        public string? Error { get; set; }

        public LoginRequestDto LoginRequest { get; set; } = new();

        public bool IsSteamEnabled { get; set; }

        public bool IsStandardEnabled { get; set; }

        private string? _Logo;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            IsSteamEnabled = BooleanValueType.GetValue((await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.AuthenticationData, SystemSettings.AuthenticationData.Steam, cancellationTokenSource.Token).ConfigureAwait(false))?.Value);
            IsStandardEnabled = BooleanValueType.GetValue((await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.AuthenticationData, SystemSettings.AuthenticationData.Standard, cancellationTokenSource.Token).ConfigureAwait(false))?.Value);

            var logoId = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.Logo, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            if (!string.IsNullOrEmpty(logoId))
            {
                _Logo = $"/api/system/image/{logoId}";
            }
        }

        public async Task ExecuteLogin()
        {
            var result = await AuthenticationStateProvider.Login(LoginRequest);

            if (result.MessageKey != null)
            {
                Snackbar.Add(Localizer.GetString(result.MessageKey, LoginRequest.UserName), result.Successful ? Severity.Success : Severity.Error);
            }

            if (result.Successful)
            {
                NavigationManager.NavigateTo(string.Empty);
            }
        }

        public void LoginWithSteam()
        {
            var returnUrl = "/register/external/complete";

            var encodedReturnUrl = HttpUtility.UrlEncode(returnUrl);

            NavigationManager.NavigateTo($"/api/auth/login/external/Steam?returnUrl={encodedReturnUrl}", true);
        }
    }
}
