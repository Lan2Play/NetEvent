using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUsers
{
    public class GetUsersResponse : ResponseBase<IEnumerable<CurrentUser>>
    {
        public GetUsersResponse(IEnumerable<CurrentUser>? value) : base(value)
        {
        }

        public GetUsersResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
