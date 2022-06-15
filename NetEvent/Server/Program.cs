using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetEvent.Server.Data;
using NetEvent.Server.Middleware;
using NetEvent.Server.Models;
using NetEvent.Server.Modules;
using NetEvent.Shared.Policy;

var builder = WebApplication.CreateBuilder(args);

switch (builder.Configuration["DBProvider"].ToLower())
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
            throw new ArgumentOutOfRangeException("DBProvider",$"DbProvider not recognized: {builder.Configuration["DBProvider"]}");
        }
}

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<NetEventUserManager>()
    .AddRoleManager<NetEventRoleManager>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.RegisterModules();

builder.Services.AddAuthorization(config => config.AddPolicies());
builder.Services.AddAuthentication().AddSteam();

builder.Services.AddRouting(options => options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        if (context.Database.IsRelational())
        {
            context.Database.Migrate();
        }
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

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(new ExceptionHandlerOptions() { AllowStatusCode404Response = true, ExceptionHandlingPath = "/error" });

app.UseEndpoints(endpoints =>
{
    endpoints.MapFallbackToFile("index.html");
});

app.MapEndpoints();

await app.RunAsync();
