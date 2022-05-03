using System.Net.Http;
using System.Threading.Tasks;
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
