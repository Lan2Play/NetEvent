using System.Collections.Generic;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemInfo
{
    public class GetSystemInfoResponse : ResponseBase<SystemInfoDto>
    {
        public GetSystemInfoResponse(SystemInfoDto value) : base(value)
        {
        }

        public GetSystemInfoResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
