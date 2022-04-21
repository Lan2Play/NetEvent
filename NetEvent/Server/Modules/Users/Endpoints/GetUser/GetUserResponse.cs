using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserResponse : ResponseBase<ApplicationUser>
    {
        public GetUserResponse(ApplicationUser value) : base(value)
        {
        }

        public GetUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
