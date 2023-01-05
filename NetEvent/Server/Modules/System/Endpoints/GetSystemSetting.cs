using System;
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
    public static class GetSystemSetting
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var settingValue = _ApplicationDbContext.Set<SystemSettingValue>().FirstOrDefault(x => request.SettingKey.Equals(x.Key));
                if (settingValue == null)
                {
                    return Task.FromResult(new Response(ReturnType.NotFound, $"Setting \"{request.SettingKey}\" not found!"));
                }

                return Task.FromResult(new Response(settingValue.ToSystemSettingValueDto()));
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(SystemSettingGroup systemSettingGroup, string settingKey)
            {
                SystemSettingGroup = systemSettingGroup;
                SettingKey = settingKey;
            }

            public SystemSettingGroup SystemSettingGroup { get; }

            public string SettingKey { get; }
        }

        public sealed class Response : ResponseBase<SystemSettingValueDto>
        {
            public Response(SystemSettingValueDto value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
