﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public static class GetUser
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _UserDbContext;

            public Handler(ApplicationDbContext userDbContext)
            {
                _UserDbContext = userDbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _UserDbContext.FindAsync<ApplicationUser>(request.Id);
                if (user == null)
                {
                    return new Response(ReturnType.NotFound, string.Empty);
                }

                var currentUser = user.ToUserDto();

                return new Response(currentUser);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(string id)
            {
                Guard.IsNotNullOrEmpty(id, nameof(id));
                Id = id;
            }

            public string Id { get; }
        }

        public sealed class Response : ResponseBase<UserDto>
        {
            public Response(UserDto value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
