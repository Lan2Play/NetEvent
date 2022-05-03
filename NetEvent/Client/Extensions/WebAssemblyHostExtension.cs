using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEvent.Client.Services;
using NetEvent.Shared.Constants;

namespace NetEvent.Client.Extensions;

public static class WebAssemblyHostExtension
{
    public async static Task SetDefaultCultureAsync(this WebAssemblyHost app)
    {
        var OrganizationDataService = app.Services.GetRequiredService<IOrganizationDataService>();
        var Logger = app.Services.GetRequiredService<ILogger<WebAssemblyHost>>();

        try
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var orgData = await OrganizationDataService.GetOrganizationDataAsync(cancellationTokenSource.Token).ConfigureAwait(false);

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
            Logger.LogError(ex, "Unable to get Culture");
        }

    }
}
