using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
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

        [Inject]
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        public RegisterRequestDto RegisterRequest { get; set; } = new();

        public async Task ExecuteRegister()
        {
            var result = await AuthenticationStateProvider.Register(RegisterRequest);

            if (result.MessageKey != null)
            {
                _Snackbar.Add(_Localizer.GetString(result.MessageKey, RegisterRequest.Email), result.Successful ? Severity.Success : Severity.Error);
            }

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}
