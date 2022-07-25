using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class DeleteSystemImage
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
                var image = await _ApplicationDbContext.SystemImages.FindAsync(new object[] { request.ImageToDelete }, cancellationToken);
                if (image == null)
                {
                    return new Response(ReturnType.NotFound, string.Empty);
                }

                var usages = _ApplicationDbContext.SystemSettingValues.AsQueryable().Where(x => x.Key != null && x.SerializedValue == image.Id).Select(x => x.Key!).ToList();
                if (usages.Any())
                {
                    return new Response(ReturnType.Error, $"Image is used in following settings: {string.Join(",", usages)}");
                }

                try
                {
                    _ApplicationDbContext.SystemImages.Remove(image);
                }
                catch (Exception e)
                {
                    return new Response(ReturnType.Error, $"Image could not be deleted: {e.Message}");
                }

                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(string imageToDelete)
            {
                ImageToDelete = imageToDelete;
            }

            public string ImageToDelete { get; }
        }

        public sealed class Response : ResponseBase
        {
            public Response() : base()
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
