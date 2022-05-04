using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

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

        public List<AdminUserDto>? AllUsers { get; private set; }

        private string? _UsersSearchString;

        // quick filter - filter gobally across multiple columns with the same input
        private Func<AdminUserDto, bool> _usersQuickFilter => x =>
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

        public List<RoleDto>? AllRoles { get; private set; }

        private string? _RoleSearchString;

        // quick filter - filter gobally across multiple columns with the same input
        private Func<RoleDto, bool> _roleQuickFilter => x =>
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

        private async Task CommittedRoleChangesAsync(RoleDto updatedRole)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await RoleService.UpdateRoleAsync(updatedRole, cancellationTokenSource.Token).ConfigureAwait(false);
        }

        #endregion
    }
}
