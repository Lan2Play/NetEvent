using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Config;

namespace NetEvent.Server.Extensions;

public static class DefaultCultureExtension
{
    public static Task SetDefaultCulture(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();

        try
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var organizationCulture = context.Set<SystemSettingValue>().Select(x => DtoMapper.Mapper.SystemSettingValueToSystemSettingValueDto(x)).ToList().Find(x => x.Key.Equals(SystemSettings.DataCultureInfo));

                    if (organizationCulture == null)
                    {
                        return Task.CompletedTask;
                    }

                    var culture = organizationCulture.Value;

                    var cultureInfo = new CultureInfo(culture);
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to get Culture");
        }

        return Task.CompletedTask;
    }
}
