using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Organization.Endpoints.PostOrganization
{
    public class PostOrganizationRequest : IRequest<PostOrganizationResponse>
    {
        public PostOrganizationRequest(OrganizationDataDto organizationData)
        {
            OrganizationData = organizationData;
        }

        public OrganizationDataDto OrganizationData { get; }
    }
}
