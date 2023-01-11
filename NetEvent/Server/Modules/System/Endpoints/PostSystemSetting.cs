using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Shared;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;
using NetEvent.Server.Data.SystemSettings;
using System;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class PostSystemSetting
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISystemSettingsManager _SystemSettingsManager;

            public Handler(ISystemSettingsManager systemSettingsManager)
            {
                _SystemSettingsManager = systemSettingsManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                var newSystemSettingValue = request.SystemSettingValue.ToSystemSettingValue();
                var result = await _SystemSettingsManager.UpdateAsync(newSystemSettingValue).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newSystemSettingValue.ToSystemSettingValueDto());

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


        public sealed class Response : ResponseBase<SystemSettingValueDto>
        {
            public Response(SystemSettingValueDto updatedSystemSetting) : base(updatedSystemSetting)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }

    }
}
