using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using NetEvent.Server;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class GetSystemInfo
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            // TODO: remove localizer as soon as it is implemented somewhere where it makes sense
            private IStringLocalizer<Localize> _Localizer { get; set; }

            // TODO: remove localizer as soon as it is implemented somewhere where it makes sense
            public Handler(IStringLocalizer<Localize> localizer)
            {
                _Localizer = localizer;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var systeminfocomponents = new List<SystemInfoComponentEntryDto>();
                var systeminfohealth = new List<SystemInfoHealthEntryDto>();
                var systeminfoversions = new List<SystemInfoVersionEntryDto>();

                AppDomain currentDomain = AppDomain.CurrentDomain;
                Assembly[] assems = currentDomain.GetAssemblies();
                foreach (Assembly assem in assems)
                {
                    systeminfocomponents.Add(new SystemInfoComponentEntryDto(assem.ManifestModule.Name, assem.ToString()));
                }

                systeminfoversions.Add(new SystemInfoVersionEntryDto("NETEVENT", Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion));
                systeminfoversions.Add(CreateSystemInfoVersionEntryFromEnv("BUILDNODE"));
                systeminfoversions.Add(CreateSystemInfoVersionEntryFromEnv("BUILDID"));
                systeminfoversions.Add(CreateSystemInfoVersionEntryFromEnv("BUILDNUMBER"));
                systeminfoversions.Add(CreateSystemInfoVersionEntryFromEnv("SOURCE_COMMIT"));

                // TODO: think about healthchecks and healthcheck modularity (to perform checks on various services like game servers, the mail server, payment apis ...) and remove dummy services
                systeminfohealth.Add(new SystemInfoHealthEntryDto("NETEVENT Server", string.Empty, true));
                systeminfohealth.Add(new SystemInfoHealthEntryDto("Email Service", "servername", false));

                var systeminfo = new SystemInfoDto(systeminfocomponents, systeminfohealth, systeminfoversions);

                 // TODO: remove localizer as soon as it is implemented somewhere where it makes sense
                Console.WriteLine(_Localizer["test.test"]);

                return Task.FromResult(new Response(systeminfo));
            }

            private static SystemInfoVersionEntryDto CreateSystemInfoVersionEntryFromEnv(string envName)
            {
                return new SystemInfoVersionEntryDto(envName, string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envName)) ? "dev" : Environment.GetEnvironmentVariable(envName));
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request()
            {
            }
        }

        public sealed class Response : ResponseBase<SystemInfoDto>
        {
            public Response(SystemInfoDto value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
