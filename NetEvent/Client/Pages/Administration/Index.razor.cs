using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public List<CurrentUserDto>? Users { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await Utils.Get<List<CurrentUserDto>>(HttpClient, "api/users");
        }
    }
}
