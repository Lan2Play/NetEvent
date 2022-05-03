using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index
    {
        [Inject]
        private IUserService UserService { get; set; } = default!;

        public List<UserDto>? Users { get; private set; }


        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            Users = await UserService.GetUsersAsync(cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }
}
