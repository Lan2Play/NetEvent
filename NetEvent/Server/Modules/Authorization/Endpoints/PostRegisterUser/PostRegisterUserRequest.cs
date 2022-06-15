using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;

public class PostRegisterUserRequest : IRequest<PostRegisterUserResponse>
{
    public PostRegisterUserRequest()
    {
    }

    public PostRegisterUserRequest(RegisterRequest registerRequest)
    {
        RegisterRequest = registerRequest;
    }

    public RegisterRequest? RegisterRequest { get; }
}
