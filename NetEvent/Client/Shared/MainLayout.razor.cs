using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager;
using NetEvent.Client.Services;
using NetEvent.Shared.Constants;

namespace NetEvent.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private IThemeService ThemeService { get; set; } = default!;

        [Inject]
        private IOrganizationDataService OrganizationDataService { get; set; } = default!;

        [Inject]
        private ILogger<MainLayout> Logger { get; set; } = default!;

        private ThemeManagerTheme _ThemeManager = new();
        bool _drawerOpen = true;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected override async Task OnInitializedAsync()
        {
            await SetThemeAsync().ConfigureAwait(false);

            await SetCultureAsync().ConfigureAwait(false);
        }

        private async Task SetThemeAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var theme = await ThemeService.GetThemeAsync(cancellationTokenSource.Token).ConfigureAwait(false);

            if (theme != null)
            {
                _ThemeManager.Theme.Palette.AppbarBackground = theme.Theme.Palette.AppbarBackground;
            }
        }

        private async Task SetCultureAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            try
            {
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
                Logger.LogError(ex, "Unable to set culture from backend.");
            }
        }

        public bool _themeManagerOpen = false;

        void ToggleThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        private async Task UpdateTheme(ThemeManagerTheme updatedTheme)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await ThemeService.UpdateThemeAsync(updatedTheme, cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }
}
