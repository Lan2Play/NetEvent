
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Organization.Endpoints.GetOrganization
{
    public class GetOrganizationResponse : ResponseBase<List<OrganizationDataDto>>
    {
        public GetOrganizationResponse(List<OrganizationDataDto> value) : base(value)
        {
        }

        public GetOrganizationResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
