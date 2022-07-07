using System;
using System.Collections.Generic;
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
            if (request.ImageName == null || request.Files.Length <= 0)
            {
                return new PostSystemImageResponse(ReturnType.Error, "Empty data is not allowed");
            }

            var imageIds = new List<string>();

            foreach (var file in request.Files)
            {
                using var ms = new MemoryStream();
                file.OpenReadStream().CopyTo(ms);
                var imageData = ms.ToArray();

                var image = new SystemImage { Name = file.FileName, Extension = Path.GetExtension(file.FileName), Data = imageData, UploadTime = DateTime.UtcNow };
                _ApplicationDbContext.SystemImages.Add(image);
                imageIds.Add(image.Id);
            }

            _ApplicationDbContext.SaveChanges();

            return new PostSystemImageResponse(imageIds);
        }
    }
}
