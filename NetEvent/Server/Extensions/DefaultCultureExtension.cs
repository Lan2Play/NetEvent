using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Shared.Config;

namespace NetEvent.Server.Extensions;

public static class DefaultCultureExtension
{
    public static async Task SetDefaultCulture(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();

        try
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {

                    var organizationCulture = await context.SystemSettingValues.Where(s => s.Key == SystemSettings.OrganizationData.DataCultureInfo).FirstAsync().ConfigureAwait(false);

                    if (organizationCulture?.SerializedValue == null)
                    {
                        return;
                    }

                    var cultureInfo = new CultureInfo(organizationCulture.SerializedValue);
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to get Culture");
        }

    }
}
