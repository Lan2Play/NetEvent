using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetEvent.Server.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}