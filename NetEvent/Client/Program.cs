using System;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using NetEvent.Client;
using NetEvent.Client.Extensions;
using NetEvent.Client.Services;
using NetEvent.Shared.Policy;

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

builder.Services.AddHttpClient(Constants.BackendApiHttpClientName)
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddHttpClient(Constants.BackendApiSecuredHttpClientName)
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 400;
    config.SnackbarConfiguration.ShowTransitionDuration = 400;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

var app = builder.Build();
await app.SetDefaultCultureAsync();

await app.RunAsync();
