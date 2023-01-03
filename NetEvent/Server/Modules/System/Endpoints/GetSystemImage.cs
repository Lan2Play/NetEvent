using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class GetSystemImage
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var image = await _ApplicationDbContext.SystemImages.FindAsync(new object[] { request.ImageName }, cancellationToken);
                if (image?.Data == null)
                {
                    var systemSettingForImage = await _ApplicationDbContext.SystemSettingValues.FindAsync(new object[] { request.ImageName }, cancellationToken);
                    if (!string.IsNullOrEmpty(systemSettingForImage?.SerializedValue))
                    {
                        image = await _ApplicationDbContext.SystemImages.FindAsync(new object[] { systemSettingForImage.SerializedValue }, cancellationToken);
                    }

                    if (image?.Data == null)
                    {
                        return new Response(ReturnType.NotFound, $"Image {request.ImageName} not found in database.");
                    }
                }

                var imageContentType = image.Extension;
                if (imageContentType != null && imageContentType.Equals("ico", StringComparison.OrdinalIgnoreCase))
                {
                    imageContentType = "x-icon";
                }
                else if (imageContentType != null && imageContentType.Equals("svg", StringComparison.OrdinalIgnoreCase))
                {
                    imageContentType = "svg+xml";
                }
                else
                {
                    // Do nothing
                }

                return new Response(Results.File(image.Data, $"image/{imageContentType}", lastModified: image.UploadTime));
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(string imageName)
            {
                ImageName = imageName;
            }

            public string ImageName { get; }
        }

        public sealed class Response : ResponseBase<IResult>
        {
            public Response(IResult fileResult) : base(fileResult)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
