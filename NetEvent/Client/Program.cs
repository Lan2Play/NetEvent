using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using NetEvent.Client;
using NetEvent.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<NetEventAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<NetEventAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrganizationDataService, OrganizationDataService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddHttpClient(Constants.BackendApiHttpClientName)
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddHttpClient(Constants.BackendApiSecuredHttpClientName)
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
