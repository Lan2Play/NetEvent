using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemImages
{
    public class GetSystemImagesHandler : IRequestHandler<GetSystemImagesRequest, GetSystemImagesResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetSystemImagesHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public Task<GetSystemImagesResponse> Handle(GetSystemImagesRequest request, CancellationToken cancellationToken)
        {
            var allImages = _ApplicationDbContext.SystemImages.AsQueryable().Select(x => DtoMapper.Mapper.SystemImageToSystemImageDto(x)).ToList();
            return Task.FromResult(new GetSystemImagesResponse(allImages));
        }
    }
}
