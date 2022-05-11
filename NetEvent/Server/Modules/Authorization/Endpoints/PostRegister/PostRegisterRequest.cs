using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;

public class PostRegisterRequest : IRequest<PostRegisterResponse>
{
    public PostRegisterRequest(RegisterRequestDto registerRequest)
    {
        RegisterRequest = registerRequest;
    }

    public RegisterRequestDto RegisterRequest { get; }
}
