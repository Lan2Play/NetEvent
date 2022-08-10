using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public static class GetUsers
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _UserDbContext;

            public Handler(ApplicationDbContext userDbContext)
            {
                _UserDbContext = userDbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allUsers = await _UserDbContext.Users.ToListAsync(cancellationToken);
                var userRoles = await _UserDbContext.UserRoles.ToListAsync(cancellationToken);
                var allRoles = await _UserDbContext.Roles.ToListAsync(cancellationToken);
                var convertedUsers = allUsers.Select(DtoMapper.ToAdminUserDto).ToList();
                var convertedRoles = allRoles.Select(DtoMapper.ToRoleDto).ToList();

                foreach (var userRole in userRoles)
                {
                    var userToSetRole = convertedUsers.First(x => x.Id.Equals(userRole.UserId, StringComparison.Ordinal));
                    var roleToSet = convertedRoles.First(x => x.Id.Equals(userRole.RoleId, StringComparison.Ordinal));
                    userToSetRole.Role = roleToSet;
                }

                return new Response(convertedUsers);
            }
        }

        public class Request : IRequest<Response>
        {
        }

        public class Response : ResponseBase<IEnumerable<UserDto>>
        {
            public Response(IEnumerable<UserDto>? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
