using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class PostSystemImage
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.ImageName == null || request.File == null)
                {
                    return new Response(ReturnType.Error, "Empty data is not allowed");
                }

                using var ms = new MemoryStream();
                await request.File.OpenReadStream().CopyToAsync(ms, cancellationToken);
                var imageData = ms.ToArray();

                var image = new SystemImage { Id = Guid.NewGuid().ToString(), Name = request.File.FileName, Extension = Path.GetExtension(request.File.FileName).Trim('.'), Data = imageData, UploadTime = DateTime.UtcNow };
                await _ApplicationDbContext.SystemImages.AddAsync(image, cancellationToken);
                await _ApplicationDbContext.SaveChangesAsync(cancellationToken);

                return new Response(image.Id);
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(string imageName, IFormFile file)
            {
                ImageName = imageName;
                File = file;
            }

            public string ImageName { get; }

            public IFormFile File { get; }
        }

        public class Response : ResponseBase<string>
        {
            public Response(string imageId) : base(imageId)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
