using MediatR;
using Microsoft.AspNetCore.Http;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;

public class PostRegisterExternalCompleteRequest : IRequest<PostRegisterExternalCompleteResponse>
{
    public PostRegisterExternalCompleteRequest(RegisterExternalCompleteRequestDto completeRegistrationRequest, HttpContext httpContext)
    {
        CompleteRegistrationRequest = completeRegistrationRequest;
        HttpContext = httpContext;
    }

    public RegisterExternalCompleteRequestDto CompleteRegistrationRequest { get; }
    
    public HttpContext HttpContext { get; }
}
