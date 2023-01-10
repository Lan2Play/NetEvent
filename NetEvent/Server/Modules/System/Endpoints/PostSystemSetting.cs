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
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.SystemSettingValue?.Key == null)
                {
                    return new Response(ReturnType.Error, "Empty key is not allowed");
                }

                var data = await _ApplicationDbContext.FindAsync<SystemSettingValue>(new object[] { request.SystemSettingValue.Key }, cancellationToken);
                if (data != null)
                {
                    data.SerializedValue = request.SystemSettingValue.Value;
                }
                else
                {
                    var serverData = request.SystemSettingValue.ToSystemSettingValue();
                    await _ApplicationDbContext.AddAsync(serverData, cancellationToken);
                }

                await _ApplicationDbContext.SaveChangesAsync(cancellationToken);

                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(SystemSettingGroup systemSettingGroup, SystemSettingValueDto systemSettingValue)
            {
                SystemSettingGroup = systemSettingGroup;
                SystemSettingValue = systemSettingValue;
            }

            public SystemSettingGroup SystemSettingGroup { get; }

            public SystemSettingValueDto SystemSettingValue { get; }
        }

        public sealed class Response : ResponseBase
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
