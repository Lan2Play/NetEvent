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
    public static class PostSystemSetting
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
                if (request.OrganizationData?.Key == null || request.OrganizationData?.Value == null)
                {
                    return new Response(ReturnType.Error, "Empty data is not allowed");
                }

                var data = await _ApplicationDbContext.FindAsync<SystemSettingValue>(new object[] { request.OrganizationData.Key }, cancellationToken);
                if (data != null)
                {
                    data.SerializedValue = request.OrganizationData.Value;
                }
                else
                {
                    var serverData = DtoMapper.Mapper.SystemSettingValueDtoToSystemSettingValue(request.OrganizationData);
                    await _ApplicationDbContext.AddAsync(serverData, cancellationToken);
                }

                await _ApplicationDbContext.SaveChangesAsync(cancellationToken);

                return new Response();
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(SystemSettingGroup systemSettingGroup, SystemSettingValueDto organizationData)
            {
                SystemSettingGroup = systemSettingGroup;
                OrganizationData = organizationData;
            }

            public SystemSettingGroup SystemSettingGroup { get; }

            public SystemSettingValueDto OrganizationData { get; }
        }

        public class Response : ResponseBase
        {
            public Response()
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
