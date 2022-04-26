using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Organization.Endpoints.PostOrganization
{
    public class PostOrganizationRequest : IRequest<PostOrganizationResponse>
    {
        public PostOrganizationRequest()
        {
        }

        public PostOrganizationRequest(OrganizationData organizationData)
        {
            OrganizationData = organizationData;
        }

        public OrganizationData OrganizationData { get; }
    }
}
