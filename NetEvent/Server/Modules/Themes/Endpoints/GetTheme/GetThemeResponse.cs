using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Themes.Endpoints.GetTheme
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
