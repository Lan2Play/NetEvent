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
    public partial class Register
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

        public RegisterRequestDto RegisterRequest { get; set; } = new();

        public bool IsSteamEnabled { get; set; }

        public bool IsStandardEnabled { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            IsSteamEnabled = BooleanValueType.GetValue((await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.AuthenticationData, SystemSettings.AuthenticationData.Steam, cancellationTokenSource.Token).ConfigureAwait(false))?.Value);
            IsStandardEnabled = BooleanValueType.GetValue((await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.AuthenticationData, SystemSettings.AuthenticationData.Standard, cancellationTokenSource.Token).ConfigureAwait(false))?.Value);
        }

        public async Task ExecuteRegister()
        {
            var result = await AuthenticationStateProvider.Register(RegisterRequest);

            if (result.MessageKey != null)
            {
                Snackbar.Add(Localizer.GetString(result.MessageKey, RegisterRequest.Email), result.Successful ? Severity.Success : Severity.Error);
            }

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/login");
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
