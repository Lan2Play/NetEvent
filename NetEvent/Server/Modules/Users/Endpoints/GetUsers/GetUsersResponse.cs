using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUsers
{
    public class GetUsersResponse : ResponseBase<IEnumerable<UserDto>>
    {
        public GetUsersResponse(IEnumerable<UserDto>? value) : base(value)
        {
        }

        public GetUsersResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
