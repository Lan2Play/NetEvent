using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager;
using NetEvent.Shared.Dto;
using Newtonsoft.Json;

namespace NetEvent.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        private ThemeManagerTheme _ThemeManager = new();
        bool _drawerOpen = true;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected override async Task OnInitializedAsync()
        {
            var theme = await HttpClient.Get<ThemeDto>("api/themes/theme");
            if (theme?.ThemeData != null)
            {
                var newThemeManager = JsonConvert.DeserializeObject<ThemeManagerTheme>(theme.ThemeData);
                if (newThemeManager != null)
                {
                    _ThemeManager.Theme.Palette.AppbarBackground = newThemeManager.Theme.Palette.AppbarBackground;
                }
            }
        }

        public bool _themeManagerOpen = false;

        void ToggleThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        private async Task UpdateTheme(ThemeManagerTheme value)
        {
            var themeData = JsonConvert.SerializeObject(value);
            await HttpClient.Put("api/themes/theme", new ThemeDto { ThemeData = themeData });
        }
    }
}