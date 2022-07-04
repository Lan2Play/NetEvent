using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;

namespace NetEvent.Client.Extensions;

[ExcludeFromCodeCoverage(Justification = "Ignore UI Extensions")]
public static class DefaultCultureExtension
{
    public static async Task SetDefaultCultureAsync(this WebAssemblyHost app)
    {
        var organizationDataService = app.Services.GetRequiredService<ISystemSettingsDataService>();
        var logger = app.Services.GetRequiredService<ILogger<WebAssemblyHost>>();

        try
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var organizationCulture = await organizationDataService.GetSystemSettingAsync(
                SystemSettingGroup.OrganizationData,
                SystemSettings.DataCultureInfo,
                newCulture =>
                {
                    if (CultureInfo.DefaultThreadCurrentCulture?.Name.Equals(newCulture.Value, StringComparison.OrdinalIgnoreCase) != true)
                    {
                        var navigationManager = app.Services.GetRequiredService<NavigationManager>();
                        navigationManager.NavigateTo(navigationManager.Uri, true);
                    }
                },
                cancellationTokenSource.Token).ConfigureAwait(false);

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
