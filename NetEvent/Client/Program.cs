using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using NetEvent.Client.Extensions;
using NetEvent.Client.Services;
using NetEvent.Shared.Policy;

namespace NetEvent.Client
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(config => config.AddPolicies());
            builder.Services.AddScoped<NetEventAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<NetEventAuthenticationStateProvider>());
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddScoped<ISystemSettingsDataService, SystemSettingsService>();
            builder.Services.AddScoped<ISystemInfoDataService, SystemInfoDataService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IVenueService, VenueService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IThemeService, ThemeService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddSingleton<NavigationService>();

            builder.Services.AddHttpClient(Constants.BackendApiHttpClientName)
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                    client.DefaultRequestVersion = HttpVersion.Version30;
                });

            builder.Services.AddHttpClient(Constants.BackendApiSecuredHttpClientName)
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                    client.DefaultRequestVersion = HttpVersion.Version30;
                })
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddMudServices(config =>
            {
#pragma warning disable S109
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 400;
                config.SnackbarConfiguration.ShowTransitionDuration = 400;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
#pragma warning restore S109
            });
            builder.Services.AddBlazoredLocalStorage();

            var app = builder.Build();
            await app.SetDefaultCultureAsync();
            await app.InitializeNavigationService();

            await app.RunAsync();
        }
    }
}
