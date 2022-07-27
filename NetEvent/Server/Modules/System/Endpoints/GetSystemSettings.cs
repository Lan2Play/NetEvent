using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class GetSystemSettings
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
                var organizationData = _ApplicationDbContext.Set<SystemSettingValue>().Select(x => DtoMapper.Mapper.SystemSettingValueToSystemSettingValueDto(x)).ToList();
                return Task.FromResult(new Response(organizationData));
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(SystemSettingGroup systemSettingGroup)
            {
                SystemSettingGroup = systemSettingGroup;
            }

            public SystemSettingGroup SystemSettingGroup { get; }
        }

        public class Response : ResponseBase<List<SystemSettingValueDto>>
        {
            public Response(List<SystemSettingValueDto> value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
