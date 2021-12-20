using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetEvent.Client;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddHttpClient("NetEvent.ServerAPI")
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

var graphQlUri = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "graphql");
var graphQlWsUri = new UriBuilder(graphQlUri);
graphQlWsUri.Scheme = "wss";

builder.Services.AddNetEventClient()
                .ConfigureHttpClient(client => client.BaseAddress = graphQlUri)
                .ConfigureWebSocketClient(client => client.Uri = graphQlWsUri.Uri);

// Supply HttpClient instances that include access tokens when making requests to the server project.
builder.Services.AddScoped(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("NetEvent.ServerAPI");
});

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.ClientId = "NetEvent-blazor-client";
    options.ProviderOptions.Authority = "https://localhost:44310/";
    options.ProviderOptions.ResponseType = "code";

    options.UserOptions.RoleClaim = "role";
    options.ProviderOptions.DefaultScopes.Add("roles");

    // Note: response_mode=fragment is the best option for a SPA. Unfortunately, the Blazor WASM
    // authentication stack is impacted by a bug that prevents it from correctly extracting
    // authorization error responses (e.g error=access_denied responses) from the URL fragment.
    // For more information about this bug, visit https://github.com/dotnet/aspnetcore/issues/28344.
    //
    options.ProviderOptions.ResponseMode = "query";
    options.AuthenticationPaths.RemoteRegisterPath = "https://localhost:44310/Identity/Account/Register";
});

await builder.Build().RunAsync();