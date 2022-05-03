using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Organization.Endpoints.GetOrganization
{
    public class GetOrganizationHandler : IRequestHandler<GetOrganizationRequest, GetOrganizationResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetOrganizationHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public Task<GetOrganizationResponse> Handle(GetOrganizationRequest request, CancellationToken cancellationToken)
        {
            var organizationData = _ApplicationDbContext.Set<OrganizationData>().Select(x => DtoMapper.Mapper.OrganizationDataToOrganizationDataDto(x)).ToList();
            return Task.FromResult(new GetOrganizationResponse(organizationData));
        }
    }
}
