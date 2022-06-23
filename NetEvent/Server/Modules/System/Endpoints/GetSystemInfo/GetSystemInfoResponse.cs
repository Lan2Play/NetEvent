using System.Collections.Generic;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemInfo
{
    public class GetSystemInfoResponse : ResponseBase<List<SystemInfoDto>>
    {
        public GetSystemInfoResponse(List<SystemInfoDto> value) : base(value)
        {
        }

        public GetSystemInfoResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
