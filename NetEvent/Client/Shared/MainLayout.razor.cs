using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager;
using System.Text.Json;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        private ThemeManagerTheme _themeManager = new ThemeManagerTheme();
        bool _drawerOpen = true;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected override async Task OnInitializedAsync()
        {
            var theme = await HttpClient.Get<Theme>("theme");
            if (theme?.ThemeData != null)
            {
                _themeGuid = theme.Id;
                var newThemeManager = JsonSerializer.Deserialize<ThemeManagerTheme>(theme.ThemeData);
                _themeManager = newThemeManager ?? _themeManager;
            }
        }

        private Guid _themeGuid;
        public bool _themeManagerOpen = false;

        void ToggleThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        async Task UpdateTheme(ThemeManagerTheme value)
        {
            _themeManager = value;
            var themeData = JsonSerializer.Serialize(_themeManager.Theme);
            await HttpClient.Put("theme", new Theme { Id = _themeGuid, ThemeData = themeData });

            if (_themeGuid == default)
            {
                var theme = await HttpClient.Get<Theme>("theme");
                _themeGuid = theme.Id;
            }
        }
    }
}