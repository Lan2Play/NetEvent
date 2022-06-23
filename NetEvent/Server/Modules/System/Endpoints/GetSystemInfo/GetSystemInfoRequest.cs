using MediatR;
using NetEvent.Shared.Config;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemInfo
{
    public class GetSystemInfoRequest : IRequest<GetSystemInfoResponse>
    {
        public GetSystemInfoRequest()
        {
            
        }

    }
}
