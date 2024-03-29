﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Profile
{
    public partial class ManageProfile
    {
        private CurrentUserDto? _CurrentUser;

        [Inject]
        private IAuthService AuthService { get; set; } = default!;

        [Inject]
        private ILogger<CompleteRegistration> Logger { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            _CurrentUser = await AuthService.GetCurrentUserInfoAsync(cancellationTokenSource.Token).ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }
    }
}
