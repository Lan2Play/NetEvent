using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.System.Endpoints.PostSystemImage
{
    public class PostSystemImageHandler : IRequestHandler<PostSystemImageRequest, PostSystemImageResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public PostSystemImageHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public async Task<PostSystemImageResponse> Handle(PostSystemImageRequest request, CancellationToken cancellationToken)
        {
            if (request.ImageName == null || request.File == null)
            {
                return new PostSystemImageResponse(ReturnType.Error, "Empty data is not allowed");
            }

            using var ms = new MemoryStream();
            await request.File.OpenReadStream().CopyToAsync(ms, cancellationToken);
            var imageData = ms.ToArray();

            var image = new SystemImage { Id = Guid.NewGuid().ToString(), Name = request.File.FileName, Extension = Path.GetExtension(request.File.FileName).Trim('.'), Data = imageData, UploadTime = DateTime.UtcNow };
            await _ApplicationDbContext.SystemImages.AddAsync(image, cancellationToken);
            await _ApplicationDbContext.SaveChangesAsync(cancellationToken);

            return new PostSystemImageResponse(image.Id);
        }
    }
}
