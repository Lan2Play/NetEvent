using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages
{
    public partial class Register
    {
        [Inject]
        private NetEventAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        public RegisterRequest RegisterRequest { get; set; } = new();

        public async Task ExecuteRegister()
        {
            try
            {
                await AuthenticationStateProvider.Register(RegisterRequest);
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                // TODO Fehlermelden
            }
        }
    }
}
