using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetThemeResponse : ResponseBase<Theme>
    {
        public GetThemeResponse(Theme? value) : base(value)
        {
        }

        public GetThemeResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
