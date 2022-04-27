using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Themes.Endpoints.PutTheme;

public class PutThemeRequest : IRequest<PutThemeResponse>
{
    public PutThemeRequest(ThemeDto theme)
    {
        Theme = theme;
    }

    public ThemeDto Theme { get; }
}
