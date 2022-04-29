using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index 
    {
        [Inject]
        private IOrganizationDataService OrganizationDataService { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        public List<UserDto>? Users { get; private set; }
        
        public List<OrganizationDataDto>? OrganizationData { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            
            Users = await UserService.GetUsersAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            OrganizationData = await OrganizationDataService.GetOrganizationDataAsync(cancellationTokenSource.Token).ConfigureAwait(false);
        }

    }
}
