using Microsoft.AspNetCore.Components;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Settings
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
        }

    }
}