using NetEvent.Shared.Models;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetUsersResponse : ResponseBase<List<ApplicationUser>>
    {
        public GetUsersResponse(List<ApplicationUser>? value) : base(value)
        {
        }

        public GetUsersResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
