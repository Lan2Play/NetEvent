using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUsers
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
