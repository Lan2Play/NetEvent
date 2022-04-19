using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints;

public class PutThemeRequest : IRequest<PutThemeResponse>
{
    public PutThemeRequest(Theme theme)
    {
        Theme = theme;
    }

    public Theme Theme { get; }
}
