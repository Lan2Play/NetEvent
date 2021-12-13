using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server;
using NetEvent.Server.Data;
using NetEvent.Server.GraphQl;
using NetEvent.Server.Models;
//using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
    options.UseOpenIddict();
},
contextLifetime: ServiceLifetime.Transient,
optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString),
    ServiceLifetime.Scoped);


builder.Services.AddGraphQLServer()
                .AddAuthorization()
                .AddInMemorySubscriptions()
                .AddQueryType<Query>()
                .AddSubscriptionType<Subscription>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<NetEventUserManager>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddAuthentication().AddSteam();



// Configure Identity to use the same JWT claims as OpenIddict instead
// of the legacy WS-Federation claims it uses by default (ClaimTypes),
// which saves you from doing the mapping in your authorization controller.
builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
});

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
//builder.Services.AddQuartz(options =>
//{
//    options.UseMicrosoftDependencyInjectionJobFactory();
//    options.UseSimpleTypeLoader();
//    options.UseInMemoryStore();
//});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
//builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddOpenIddict()
    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        options//.ReplaceApplicationStoreResolver<NetEventApplicationStoreResolver>()
                .UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();

        // Enable Quartz.NET integration.
        options.UseQuartz();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the authorization, logout, token and userinfo endpoints.
        options.SetAuthorizationEndpointUris("/connect/authorize")
               .SetLogoutEndpointUris("/connect/logout")
               .SetTokenEndpointUris("/connect/token")
               .SetUserinfoEndpointUris("/connect/userinfo");

        // Mark the "email", "profile" and "roles" scopes as supported scopes.
        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

        // Note: the sample uses the code and refresh token flows but you can enable
        // the other flows if you need to support implicit, password or client credentials.
        options.AllowAuthorizationCodeFlow()
               .AllowRefreshTokenFlow();

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
       .EnableAuthorizationEndpointPassthrough()
       .EnableLogoutEndpointPassthrough()
       .EnableStatusCodePagesIntegration()
       .EnableTokenEndpointPassthrough();
    })

    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        //options.SetIssuer("http://localhost:58795/");
        //options.AddAudiences("resource_server_1");
        //options.UseIntrospection()
        //                 .SetClientId("NetEvent-blazor-client")
        //                 .SetClientSecret("846B62D0-DEF9-4215-A99D-86E6B8DAB342");
        //options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register the worker responsible of seeding the database with the sample clients.
// Note: in a real world application, this step should be part of a setup script.
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseWebSockets();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
    endpoints.MapGraphQL();
});

await app.RunAsync();