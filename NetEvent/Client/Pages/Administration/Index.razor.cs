using Microsoft.AspNetCore.Components;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index 
    {

        [Inject]
        public HttpClient HttpClient { get; set; }

        public List<CurrentUser>? Users { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await Utils.Get<List<CurrentUser>>(HttpClient, "users");
        }

    }
}