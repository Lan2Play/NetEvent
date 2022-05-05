using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;
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

        public LoginRequest LoginRequest { get; set; } = new();

        public string Error { get; set; }

        public async Task ExecuteLogin()
        {
            try
            {
                await AuthenticationStateProvider.Login(LoginRequest);
                NavigationManager.NavigateTo(string.Empty);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        public void LoginWithSteam()
        {
            var returnUrl = "/completeregistration";

            var encodedReturnUrl = HttpUtility.UrlEncode(returnUrl);

            
            NavigationManager.NavigateTo($"/api/auth/login/external/Steam?returnUrl={encodedReturnUrl}", true);
        }
    }
}
