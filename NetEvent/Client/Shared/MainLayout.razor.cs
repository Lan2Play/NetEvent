﻿using System;
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
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        private readonly ThemeManagerTheme _ThemeManager = new();

        private string? _OrganizationName;
        private bool _HideOrganizationNameInNavBar;
        private bool _DrawerVisible;
        private bool _DrawerOpen = true;
        private string? _Logo;

        private void DrawerToggle()
        {
            _DrawerOpen = !_DrawerOpen;
        }

        protected override async Task OnInitializedAsync()
        {
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
            UpdateDraweVisibility();

            using var cancellationTokenSource = new CancellationTokenSource();
            _OrganizationName = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.OrganizationName, OrganizationNameChanged, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;

            var hideOrganizationNameInNavBarDto = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.HideOrganizationNameInNavBar, HideOrganizationNameInNavBarChanged, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            _HideOrganizationNameInNavBar = BooleanValueType.GetValue(hideOrganizationNameInNavBarDto);

            var logoId = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.Logo, LogoIdChanged, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            if (!string.IsNullOrEmpty(logoId))
            {
                _Logo = $"/api/system/image/{logoId}";
            }
        }

        private void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            UpdateDraweVisibility();
        }

        private void UpdateDraweVisibility()
        {
            var relativeUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            _DrawerVisible = relativeUri.StartsWith("Administration", StringComparison.OrdinalIgnoreCase);
            StateHasChanged();
        }

        private void OrganizationNameChanged(SystemSettingValueDto settingValue)
        {
            _OrganizationName = settingValue.Value;
            StateHasChanged();
        }

        private void HideOrganizationNameInNavBarChanged(SystemSettingValueDto settingValue)
        {
            _HideOrganizationNameInNavBar = BooleanValueType.GetValue(settingValue.Value);
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
    }
}
