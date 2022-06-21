using System.Collections.Generic;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemSettings
{
    public class GetSystemSettingsResponse : ResponseBase<List<SystemSettingValueDto>>
    {
        public GetSystemSettingsResponse(List<SystemSettingValueDto> value) : base(value)
        {
        }

        public GetSystemSettingsResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
