using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles.Endpoints
{
    public static class GetRoles
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly NetEventRoleManager _RoleManager;

            public Handler(NetEventRoleManager roleManager)
            {
                _RoleManager = roleManager;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allRoles = _RoleManager.Roles.ToList();
                var roleDtos = allRoles.Select(async role =>
                {
                    var roleDto = role.ToRoleDto();
                    var roleClaims = await _RoleManager.GetClaimsAsync(role);
                    roleDto.Claims = roleClaims.Select(roleClaim => roleClaim.Type).ToList();
                    return roleDto;
                }).Select(t => t.Result).ToList();

                return Task.FromResult(new Response(roleDtos));
            }
        }

        public sealed class Request : IRequest<Response>
        {
        }

        public sealed class Response : ResponseBase<IReadOnlyCollection<RoleDto>>
        {
            public Response(IReadOnlyCollection<RoleDto> value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
