using MediatR;
using Microsoft.AspNetCore.Http;

namespace NetEvent.Server.Modules.System.Endpoints.PostSystemImage
{
    public class PostSystemImageRequest : IRequest<PostSystemImageResponse>
    {
        public PostSystemImageRequest(string imageName, IFormFile file)
        {
            ImageName = imageName;
            File = file;
        }

        public string ImageName { get; }

        public IFormFile File { get; }
    }
}
