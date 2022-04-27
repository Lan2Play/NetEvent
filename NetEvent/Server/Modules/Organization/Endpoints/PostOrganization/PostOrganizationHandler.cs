using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Organization.Endpoints.PostOrganization
{
    public class PostOrganizationHandler : IRequestHandler<PostOrganizationRequest, PostOrganizationResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public PostOrganizationHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public async Task<PostOrganizationResponse> Handle(PostOrganizationRequest request, CancellationToken cancellationToken)
        {
            var data = await _ApplicationDbContext.FindAsync<OrganizationData>(new object[] { request.OrganizationData.Key }, cancellationToken);
            if (data != null)
            {
                _ApplicationDbContext.Update(new OrganizationData(data.Key, request.OrganizationData.Value));
            }
            else
            {
                var serverData = DtoMapper.Mapper.OrganizationDataDtoToOrganizationData(request.OrganizationData);
                await _ApplicationDbContext.AddAsync(serverData, cancellationToken);
            }

            await _ApplicationDbContext.SaveChangesAsync();

            return new PostOrganizationResponse();
        }
    }
}
