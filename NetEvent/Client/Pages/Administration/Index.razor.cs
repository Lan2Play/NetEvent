using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Administration;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index
    {
        [Inject]
        private IUserService UserService { get; set; } = default!;

        public List<AdminUserDto>? Users { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            Users = await UserService.GetUsersAsync(cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }
}
