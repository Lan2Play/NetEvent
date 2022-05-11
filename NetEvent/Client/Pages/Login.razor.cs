using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Services;
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
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        public LoginRequestDto LoginRequest { get; set; } = new();

        public async Task ExecuteLogin()
        {
            var result = await AuthenticationStateProvider.Login(LoginRequest);

            if (result.MessageKey != null)
            {
                _Snackbar.Add(_Localizer.GetString(result.MessageKey, LoginRequest.UserName), result.Successful ? Severity.Success : Severity.Error);
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
