using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;

public class PostCompleteRegistrationRequest : IRequest<PostCompleteRegistrationResponse>
{
    public PostCompleteRegistrationRequest(CompleteRegistrationRequestDto completeRegistrationRequest)
    {
        CompleteRegistrationRequest = completeRegistrationRequest;
    }

    public CompleteRegistrationRequestDto CompleteRegistrationRequest { get; }
}
