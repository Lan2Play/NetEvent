using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class GetSystemImages
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allImages = _ApplicationDbContext.SystemImages.AsQueryable().Select(DtoMapper.ToSystemImageDto).ToList();

                var result = new List<SystemImageWithUsagesDto>();
                foreach (var image in allImages)
                {
                    var settingUsages = _ApplicationDbContext.SystemSettingValues.AsQueryable().Where(x => x.Key != null && x.SerializedValue == image.Id).Select(x => x.Key!).ToList();
                    var eventUsage = _ApplicationDbContext.Events.AsQueryable().Where(x => CheckIfImageIsUsedInEvent(x, image)).Select(x => x.Id!.ToString()!).ToList();
                    result.Add(new SystemImageWithUsagesDto(image, settingUsages, eventUsage));
                }

                return Task.FromResult(new Response(result));
            }

            private static bool CheckIfImageIsUsedInEvent(Event e, SystemImageDto image)
            {
                if (image.Id == null)
                {
                    return false;
                }

                return (e.Description != null && e.Description.Contains(image.Id, StringComparison.OrdinalIgnoreCase))
                    || (e.ShortDescription != null && e.ShortDescription.Contains(image.Id, StringComparison.OrdinalIgnoreCase));
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request()
            {
            }
        }

        public sealed class Response : ResponseBase<IEnumerable<SystemImageWithUsagesDto>>
        {
            public Response(IEnumerable<SystemImageWithUsagesDto> result) : base(result)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
