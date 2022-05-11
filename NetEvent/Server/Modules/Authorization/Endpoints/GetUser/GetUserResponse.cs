using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetUserResponse : ResponseBase<CurrentUserDto>
    {
        public GetUserResponse(CurrentUserDto? value) : base(value)
        {
        }

        public GetUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
