using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Users
    {
        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IRoleService RoleService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            AllUsers = await UserService.GetUsersAsync(cancellationTokenSource.Token);
            AllRoles = await RoleService.GetRolesAsync(cancellationTokenSource.Token);
        }

        #region Users

        public List<UserDto>? AllUsers { get; private set; }

        private string? _UsersSearchString;

        // quick filter - filter gobally across multiple columns with the same input
        private Func<UserDto, bool> _usersQuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_UsersSearchString))
            {
                return true;
            }

            if (x.UserName.Contains(_UsersSearchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (x.FirstName.Contains(_UsersSearchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (x.LastName.Contains(_UsersSearchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (x.Email.Contains(_UsersSearchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        };

        private async Task CommittedUserChangesAsync(UserDto updatedUser)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await UserService.UpdateUserAsync(updatedUser, cancellationTokenSource.Token).ConfigureAwait(false);
        }

        #endregion

        #region Roles

        public List<IdentityRole>? AllRoles { get; private set; }

        private string? _RoleSearchString;

        // quick filter - filter gobally across multiple columns with the same input
        private Func<IdentityRole, bool> _roleQuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_UsersSearchString))
            {
                return true;
            }

            if (x.Name.Contains(_UsersSearchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        };

        private async Task CommittedRoleChangesAsync(IdentityRole updatedRole)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await RoleService.UpdateRoleAsync(updatedRole, cancellationTokenSource.Token).ConfigureAwait(false);
        }

        #endregion
    }
}
