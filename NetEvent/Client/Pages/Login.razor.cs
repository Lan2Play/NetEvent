using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages
{
    public partial class Login
    {
        [Inject]
        public CustomStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public LoginRequest LoginRequest { get; set; } = new();

        public string Error { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }

        public async Task ExecuteLogin()
        {
            try
            {
                await AuthenticationStateProvider.Login(LoginRequest);
                NavigationManager.NavigateTo("");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
    }
}
