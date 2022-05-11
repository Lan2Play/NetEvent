using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;

public class PostRegisterExternalCompleteRequest : IRequest<PostRegisterExternalCompleteResponse>
{
    public PostRegisterExternalCompleteRequest(RegisterExternalCompleteRequestDto completeRegistrationRequest)
    {
        CompleteRegistrationRequest = completeRegistrationRequest;
    }

    public RegisterExternalCompleteRequestDto CompleteRegistrationRequest { get; }
}
