using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager;
using NetEvent.Client.Services;

namespace NetEvent.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private IThemeService ThemeService { get; set; } = default!;

        private ThemeManagerTheme _ThemeManager = new();
        bool _drawerOpen = true;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected override async Task OnInitializedAsync()
        {
            await SetThemeAsync().ConfigureAwait(false);
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
