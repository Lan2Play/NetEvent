using Microsoft.AspNetCore.Components;
using NetEvent.Shared.Models;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index 
    {

        [Inject]
        public HttpClient HttpClient { get; set; }

        public List<ApplicationUser>? Users { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await Utils.Get<List<ApplicationUser>>(HttpClient, "users");
        }

    }
}