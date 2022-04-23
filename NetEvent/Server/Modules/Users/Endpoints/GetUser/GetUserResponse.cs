using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserResponse : ResponseBase<User>
    {
        public GetUserResponse(User value) : base(value)
        {
        }

        public GetUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
