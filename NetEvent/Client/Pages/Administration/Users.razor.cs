using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using NetEvent.Client;
using NetEvent.Client.Shared;
using MudBlazor;
using NetEvent.Shared.Models;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Users
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public List<ApplicationUser>? AllUsers { get; private set; }
        private string _searchString;

        protected override async Task OnInitializedAsync()
        {
            AllUsers = await Utils.Get<List<ApplicationUser>>(HttpClient, "users");
        }


        // quick filter - filter gobally across multiple columns with the same input
        private Func<ApplicationUser, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.UserName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.FirstName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.LastName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };


        void CommittedItemChanges(ApplicationUser item)
        {
            _ = Utils.Put(HttpClient, $"users/{item.Id}", item);
        }
    }
}