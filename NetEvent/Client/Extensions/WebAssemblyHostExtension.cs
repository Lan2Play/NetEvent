using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Constants;

namespace NetEvent.Client.Extensions;

public static class WebAssemblyHostExtension
{
    public static async Task SetDefaultCultureAsync(this WebAssemblyHost app)
    {
        var organizationDataService = app.Services.GetRequiredService<ISystemSettingsDataService>();
        var logger = app.Services.GetRequiredService<ILogger<WebAssemblyHost>>();

        try
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var orgData = await organizationDataService.GetSystemSettingsAsync(SystemSettingGroup.OrganizationData, cancellationTokenSource.Token).ConfigureAwait(false);

            var organizationCulture = orgData.FirstOrDefault(a => a.Key.Equals(OrganizationDataConstants.CultureKey));

            if (organizationCulture == null)
            {
                return;
            }

            var culture = organizationCulture.Value;

            var cultureInfo = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to get Culture");
        }
    }
}
