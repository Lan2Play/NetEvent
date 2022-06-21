using MediatR;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints.PostOrganization
{
    public class PostSystemSettingsRequest : IRequest<PostSystemSettingsResponse>
    {
        public PostSystemSettingsRequest(SystemSettingGroup systemSettingGroup, SystemSettingValueDto organizationData)
        {
            SystemSettingGroup = systemSettingGroup;
            OrganizationData = organizationData;
        }

        public SystemSettingGroup SystemSettingGroup { get; }
        public SystemSettingValueDto OrganizationData { get; }
    }
}
