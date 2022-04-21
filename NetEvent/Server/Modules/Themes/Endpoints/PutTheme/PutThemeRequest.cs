using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Themes.Endpoints.PutTheme;

public class PutThemeRequest : IRequest<PutThemeResponse>
{
    public PutThemeRequest(Theme theme)
    {
        Theme = theme;
    }

    public Theme Theme { get; }
}
