using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;

        public GetUsersHandler(ApplicationDbContext userDbContext)
        {
            _UserDbContext = userDbContext;
        }

        public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var allUsers = await _UserDbContext.Users.ToListAsync(cancellationToken);
            var userRoles = await _UserDbContext.UserRoles.ToListAsync(cancellationToken);
            var allRoles = await _UserDbContext.Roles.ToListAsync(cancellationToken);
            var convertedUsers = allUsers.Select(DtoMapper.Mapper.ApplicaitonUserToAdminUserDto).ToList();
            var convertedRoles = allRoles.Select(DtoMapper.Mapper.IdentityRoleToRoleDto).ToList();

            foreach (var userRole in userRoles)
            {
                var userToSetRole = convertedUsers.First(x => x.Id.Equals(userRole.UserId, StringComparison.Ordinal));
                var roleToSet = convertedRoles.First(x => x.Id.Equals(userRole.RoleId, StringComparison.Ordinal));
                userToSetRole.Role = roleToSet;
            }

            return new GetUsersResponse(convertedUsers);
        }
    }
}
