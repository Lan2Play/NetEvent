using MediatR;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemInfo
{
    public class GetSystemInfoRequest : IRequest<GetSystemInfoResponse>
    {
        public GetSystemInfoRequest()
        {
        }
    }
}
