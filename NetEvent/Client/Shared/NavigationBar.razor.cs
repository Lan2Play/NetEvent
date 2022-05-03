using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace NetEvent.Client.Shared
{
    public partial class NavigationBar
    {
        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.Logout();
            Navigation.NavigateTo("/");
        }
    }
}
