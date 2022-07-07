using MediatR;
using Microsoft.AspNetCore.Http;

namespace NetEvent.Server.Modules.System.Endpoints.PostSystemImage
{
    public class PostSystemImageRequest : IRequest<PostSystemImageResponse>
    {
        public PostSystemImageRequest(string imageName, IFormFile[] files)
        {
            ImageName = imageName;
            Files = files;
        }

        public string ImageName { get; }

        public IFormFile[] Files { get; }
    }
}
