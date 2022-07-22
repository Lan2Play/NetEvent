using MediatR;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemImages
{
    public class GetSystemImagesRequest : IRequest<GetSystemImagesResponse>
    {
        public GetSystemImagesRequest()
        {
        }
    }
}
