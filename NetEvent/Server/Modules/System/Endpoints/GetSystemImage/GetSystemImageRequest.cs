using MediatR;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemImage
{
    public class GetSystemImageRequest : IRequest<GetSystemImageResponse>
    {
        public GetSystemImageRequest(string imageName)
        {
            ImageName = imageName;
        }

        public string ImageName { get; }
    }
}
