using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Organization.Endpoints.GetOrganization
{
    public class GetOrganizationResponse : ResponseBase<List<OrganizationData>>
    {
        public GetOrganizationResponse(List<OrganizationData> value) : base(value)
        {
        }

        public GetOrganizationResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
