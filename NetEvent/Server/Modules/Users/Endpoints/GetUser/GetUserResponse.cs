using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserResponse : ResponseBase<CurrentUser>
    {
        public GetUserResponse(CurrentUser value) : base(value)
        {
        }

        public GetUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
