using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Roles.Endpoints.PutRole;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemImage
{
    public class GetSystemImageHandler : IRequestHandler<GetSystemImageRequest, GetSystemImageResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetSystemImageHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public async Task<GetSystemImageResponse> Handle(GetSystemImageRequest request, CancellationToken cancellationToken)
        {
            var image = await _ApplicationDbContext.SystemImages.FindAsync(new object[] { request.ImageName }, cancellationToken);
            if (image == null)
            {
                var systemSettingForImage = await _ApplicationDbContext.SystemSettingValues.FindAsync(new object[] { request.ImageName }, cancellationToken);
                if (!string.IsNullOrEmpty(systemSettingForImage?.SerializedValue))
                {
                    image = await _ApplicationDbContext.SystemImages.FindAsync(new object[] { systemSettingForImage.SerializedValue }, cancellationToken);
                }

                if (image == null)
                {
                    return new GetSystemImageResponse(ReturnType.NotFound, $"Image {request.ImageName} not found in database.");
                }
            }

            return new GetSystemImageResponse(Results.File(image.Data, $"image/{image.Extension}", lastModified: image.UploadTime));
        }
    }
}
