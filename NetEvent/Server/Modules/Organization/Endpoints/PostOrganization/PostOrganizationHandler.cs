using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Organization.Endpoints.PostOrganization
{
    public class PostOrganizationHandler : IRequestHandler<PostOrganizationRequest, PostOrganizationResponse>
    {
        private readonly ILogger<PostOrganizationHandler> _Logger;
        private readonly ApplicationDbContext _ApplicationDbContext;

        public PostOrganizationHandler(ILogger<PostOrganizationHandler> logger, ApplicationDbContext applicationDbContext)
        {
            _Logger = logger;
            _ApplicationDbContext = applicationDbContext;
        }

        public async Task<PostOrganizationResponse> Handle(PostOrganizationRequest request, CancellationToken cancellationToken)
        {
            var data = await _ApplicationDbContext.FindAsync<Models.OrganizationData>(new object[] { request.OrganizationData.Key }, cancellationToken);
            if (data != null)
            {
                data.Value = request.OrganizationData.Value;
            }
            else
            {
                var serverData = DtoMapper.Mapper.DtoOrganizationDataToOrganizationData(request.OrganizationData);
                await _ApplicationDbContext.AddAsync(serverData, cancellationToken);
            }

            return new PostOrganizationResponse();
        }
    }
}
