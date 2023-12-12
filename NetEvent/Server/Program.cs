using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NetEvent.Server.Configuration;
using NetEvent.Server.Data;
using NetEvent.Server.Data.Events;
using NetEvent.Server.Middleware;
using NetEvent.Server.Models;
using NetEvent.Server.Modules;
using NetEvent.Server.Services;
using NetEvent.Shared.Policy;
using Slugify;

const int _DefaultPort = 5001;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration == null)
{
    return;
}

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(_DefaultPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});

switch (builder.Configuration["DBProvider"]?.ToLower(CultureInfo.InvariantCulture))
{
    case "sqlite":
        {
            builder.Services.AddDbContext<ApplicationDbContext, SqliteApplicationDbContext>();
            builder.Services.AddHealthChecks().AddDbContextCheck<SqliteApplicationDbContext>();
        }

        break;
    case "psql":
        {
            builder.Services.AddDbContext<ApplicationDbContext, PsqlApplicationDbContext>();
            builder.Services.AddHealthChecks().AddDbContextCheck<PsqlApplicationDbContext>();
        }

        break;
    default:
        {
            throw new NotSupportedException($"DbProvider not recognized: {builder.Configuration["DBProvider"]}");
        }
}

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.AllowedUserNameCharacters = string.Empty;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<NetEventUserManager>()
    .AddRoleManager<NetEventRoleManager>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthentication().AddSteam(options =>
{
    options.ApplicationKey = builder.Configuration?.GetSection("SteamConfig").Get<SteamConfig>()?.ApplicationKey;
});
builder.Services.AddAuthorization(config =>
{
    config.AddPolicies();
});

builder.Services.RegisterModules();

builder.Services.AddRouting(options => options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventManager, EventManager>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPaymentManager, PaymentManager>();
builder.Services.AddScoped<ISlugHelper, SlugHelper>();

builder.Services.AddSingleton<IEmailRenderer, RazorEmailRenderer>();

var emailConfig = builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>();

if (emailConfig?.SendGridConfig != null)
{
    builder.Services.TryAddSingleton(emailConfig.SendGridConfig);
    builder.Services.TryAddScoped<IEmailSender, SendGridEmailSender>();
}
else if (emailConfig?.SmtpConfig != null)
{
    builder.Services.TryAddSingleton(emailConfig.SmtpConfig);
    builder.Services.TryAddScoped<IEmailSender, SmtpEmailSender>();
}
else
{
    builder.Services.TryAddScoped<IEmailSender, NullEmailSender>();
}

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.WebHost.UseStaticWebAssets();

var app = builder.Build();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (context.Database.IsRelational())
    {
        context.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHealthChecks("/healthz");
    app.UseHsts();
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.MapFallbackToFile("index.html");

app.MapEndpoints();

await app.RunAsync();

static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(HttpStatusCode statusCode, Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
    context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = (int)statusCode;
            return Task.CompletedTask;
        }

        return existingRedirector(context);
    };
