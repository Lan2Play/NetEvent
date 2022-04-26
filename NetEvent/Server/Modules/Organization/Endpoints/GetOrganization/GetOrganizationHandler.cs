using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Organization.Endpoints.GetOrganization
{
    public class GetOrganizationHandler : IRequestHandler<GetOrganizationRequest, GetOrganizationResponse>
    {
        private readonly ILogger<GetOrganizationHandler> _Logger;
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetOrganizationHandler(ILogger<GetOrganizationHandler> logger, ApplicationDbContext applicationDbContext)
        {
            _Logger = logger;
            _ApplicationDbContext = applicationDbContext;
        }

        public Task<GetOrganizationResponse> Handle(GetOrganizationRequest request, CancellationToken cancellationToken)
        {
            var organizationData = _ApplicationDbContext.Set<OrganizationData>().Select(x => DtoMapper.Mapper.OrganizationDataToDtoOrganizationData(x)).ToList();
            return Task.FromResult(new GetOrganizationResponse(organizationData));
        }
    }
}
