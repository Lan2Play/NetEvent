using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Users
    {
        [Inject]
        private IUserService _UserService { get; set; } = default!;

        [Inject]
        private IRoleService _RoleService { get; set; } = default!;

        [Inject]
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        private NetEventDataGrid<RoleDto> RolesDataGrid;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            AllUsers = await _UserService.GetUsersAsync(cancellationTokenSource.Token);
            AllRoles = await _RoleService.GetRolesAsync(cancellationTokenSource.Token);
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

        private async Task CommittedUserChangesAsync(AdminUserDto updatedUser)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var result = await _UserService.UpdateUserAsync(updatedUser, cancellationTokenSource.Token).ConfigureAwait(false);
            if (result.Successful)
            {
                result = await _UserService.UpdateUserRoleAsync(updatedUser.Id, updatedUser.Role.Id, cancellationTokenSource.Token).ConfigureAwait(false);
            }

            if (result.MessageKey != null)
            {
                _Snackbar.Add(_Localizer.GetString(result.MessageKey, updatedUser.Email), result.Successful ? Severity.Success : Severity.Error);
            }
        }
        #endregion

        #region Roles

        public List<RoleDto>? AllRoles { get; private set; }

        public RoleDto? SelectedRole { get; private set; }

        private string? _RoleSearchString;

        private string? SelectionLabelValue { get; set; }

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

            if (updatedRole.Id != null)
            {
                await _RoleService.UpdateRoleAsync(updatedRole, cancellationTokenSource.Token).ConfigureAwait(false);
            }
            else
            {
                await _RoleService.AddRoleAsync(updatedRole, cancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        private string CreateSelectionLabel(List<string> selectedValues)
        {
            switch (selectedValues.Count)
            {
                case int n when n == 1:
                    return $"{selectedValues.Count} {_Localizer["Administration.Users.Roles.SelectPermissionSingular"]}";

                case int n when n > 1:
                    return $"{selectedValues.Count} {_Localizer["Administration.Users.Roles.SelectPermissionPlural"]}";

                default:
                    return _Localizer["Administration.Users.Roles.NothingSelected"];
            }
        }
        #endregion
    }
}
