using System.Net.Http;
using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager;

namespace NetEvent.Client.Pages.Administration
{
    public partial class AdminMenu
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        private ThemeManagerTheme _themeManager = new ThemeManagerTheme();
     
    }
}
