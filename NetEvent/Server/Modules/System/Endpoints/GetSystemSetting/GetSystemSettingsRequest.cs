using MediatR;
using NetEvent.Shared.Config;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemSettings
{
    public class GetSystemSettingsRequest : IRequest<GetSystemSettingsResponse>
    {
        public GetSystemSettingsRequest(SystemSettingGroup systemSettingGroup)
        {
            SystemSettingGroup = systemSettingGroup;
        }

        public SystemSettingGroup SystemSettingGroup { get; }
    }
}
