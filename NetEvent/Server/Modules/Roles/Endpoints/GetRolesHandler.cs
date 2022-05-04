using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Roles.Endpoints
{
    public class GetRolesHandler : IRequestHandler<GetRolesRequest, GetRolesResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetRolesHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public Task<GetRolesResponse> Handle(GetRolesRequest request, CancellationToken cancellationToken)
        {
            var allRoles = _ApplicationDbContext.Roles.ToList();
            return Task.FromResult(new GetRolesResponse(allRoles.Select(x => DtoMapper.Mapper.IdentityRoleToRoleDto(x)).ToList()));
        }
    }
}
