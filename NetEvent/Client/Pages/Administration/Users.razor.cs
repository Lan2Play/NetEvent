using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Users
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AllUsers = await Utils.Get<List<UserDto>>(HttpClient, "api/users");
            AllRoles = await Utils.Get<List<IdentityRole>>(HttpClient, "roles");
        }

        #region Users

        public List<UserDto>? AllUsers { get; private set; }
        private string _usersSearchString;

        // quick filter - filter gobally across multiple columns with the same input
        private Func<UserDto, bool> _usersQuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_usersSearchString))
                return true;

            if (x.UserName.Contains(_usersSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.FirstName.Contains(_usersSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.LastName.Contains(_usersSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Email.Contains(_usersSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };


        void CommittedUserChanges(UserDto item)
        {
            _ = Utils.Put(HttpClient, $"api/users/{item.Id}", item);
        }

        #endregion

        #region Roles

        public List<IdentityRole>? AllRoles { get; private set; }

        private string _roleSearchString;

        // quick filter - filter gobally across multiple columns with the same input
        private Func<IdentityRole, bool> _roleQuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_usersSearchString))
                return true;

            if (x.Name.Contains(_usersSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };


        void CommittedRoleChanges(IdentityRole item)
        {
            _ = Utils.Put(HttpClient, $"role/{item.Id}", item);
        }

        #endregion
    }
}
