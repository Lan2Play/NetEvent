using MediatR;
using Microsoft.AspNetCore.Http;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;

public class PostRegisterRequest : IRequest<PostRegisterResponse>
{
    public PostRegisterRequest(RegisterRequestDto registerRequest, HttpContext httpContext)
    {
        RegisterRequest = registerRequest;
        HttpContext = httpContext;
    }

    public RegisterRequestDto RegisterRequest { get; }
    public HttpContext HttpContext { get; }
}
