using MediatR;

namespace NetEvent.Server.Modules.Organization.Endpoints.GetOrganization
{
    public class GetOrganizationRequest : IRequest<GetOrganizationResponse>
    {
        public GetOrganizationRequest()
        {
        }
    }
}
