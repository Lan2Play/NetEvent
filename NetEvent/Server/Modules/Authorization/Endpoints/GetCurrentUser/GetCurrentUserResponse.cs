using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetCurrentUserResponse : ResponseBase<CurrentUserDto>
    {
        public GetCurrentUserResponse(CurrentUserDto? value) : base(value)
        {
        }

        public GetCurrentUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
