using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private IThemeService ThemeService { get; set; } = default!;

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        private readonly ThemeManagerTheme _ThemeManager = new();

        private string? _OrganizationName;
        private bool _drawerOpen = true;
        private string? _Logo;

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            _OrganizationName = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationName, OrganizationNameChanged, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;

            var logoId = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.Logo, LogoIdChanged, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            if (!string.IsNullOrEmpty(logoId))
            {
                _Logo = $"/api/system/image/{logoId}";
            }

            await SetThemeAsync().ConfigureAwait(false);
        }

        private void OrganizationNameChanged(SystemSettingValueDto settingValue)
        {
            _OrganizationName = settingValue.Value;
            StateHasChanged();
        }

        private void LogoIdChanged(SystemSettingValueDto newLogoValue)
        {
            if (!string.IsNullOrEmpty(newLogoValue.Value))
            {
                _Logo = $"/api/system/image/{newLogoValue.Value}";
                StateHasChanged();
            }
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

        private async Task UpdateTheme(ThemeManagerTheme updatedTheme)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await ThemeService.UpdateThemeAsync(updatedTheme, cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }
}
