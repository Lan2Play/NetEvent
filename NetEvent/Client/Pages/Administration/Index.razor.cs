using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index 
    {

        [Inject]
        public HttpClient HttpClient { get; set; }
        
        [Inject]
        public OrganizationDataService OrganizationDataService { get; set; }

        public List<CurrentUserDto>? Users { get; private set; }
        public List<OrganizationDataDto>? OrganizationData { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await Utils.Get<List<CurrentUserDto>>(HttpClient, "api/users");
            OrganizationData = await OrganizationDataService.GetOrganizationData();
        }

    }
}
