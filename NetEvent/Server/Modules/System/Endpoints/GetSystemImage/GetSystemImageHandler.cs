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
                image = _ApplicationDbContext.SystemImages.FirstOrDefault(i => (i.Id != null && i.Id.Equals(request.ImageName, StringComparison.OrdinalIgnoreCase)) || i.Name.Equals(request.ImageName, StringComparison.OrdinalIgnoreCase));

                if (image == null)
                {
                    return new GetSystemImageResponse(ReturnType.NotFound, $"Image {request.ImageName} not found in database.");
                }
            }

            return new GetSystemImageResponse(Results.File(image.Data, fileDownloadName: image.Name, lastModified: image.UploadTime));
        }
    }
}
