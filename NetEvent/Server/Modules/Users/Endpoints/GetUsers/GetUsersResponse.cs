using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUsers
{
    public class GetUsersResponse : ResponseBase<IEnumerable<User>>
    {
        public GetUsersResponse(IEnumerable<User>? value) : base(value)
        {
        }

        public GetUsersResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
