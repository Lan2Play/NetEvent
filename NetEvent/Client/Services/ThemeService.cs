using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MudBlazor.ThemeManager;
using NetEvent.Shared.Dto;
using Newtonsoft.Json;

namespace NetEvent.Client.Services
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Services")]
    public class ThemeService : IThemeService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<ThemeService> _Logger;

        public ThemeService(IHttpClientFactory httpClientFactory, ILogger<ThemeService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<ThemeManagerTheme?> GetThemeAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var themeDto = await client.GetFromJsonAsync<ThemeDto>("api/themes/theme", cancellationToken).ConfigureAwait(false);

                if (themeDto?.ThemeData == null)
                {
                    return null;
                }

                var newThemeManager = JsonConvert.DeserializeObject<ThemeManagerTheme>(themeDto.ThemeData);

                return newThemeManager;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get theme from backend.");
            }

            return null;
        }

        public async Task<bool> UpdateThemeAsync(ThemeManagerTheme updatedTheme, CancellationToken cancellationToken)
        {
            try
            {
                var themeData = JsonConvert.SerializeObject(updatedTheme);
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var content = JsonContent.Create(new ThemeDto { ThemeData = themeData });

                var response = await client.PutAsync("api/themes/theme", content, cancellationToken);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update theme in backend.");
            }

            return false;
        }
    }
}
