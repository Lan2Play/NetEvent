using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class GetSystemInfo
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            public Handler()
            {
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
                    systeminfocomponents.Add(new SystemInfoComponentEntryDto(assem.ManifestModule.Name.ToString(), assem.ToString()));
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

                return Task.FromResult(new Response(systeminfo));
            }

            private static SystemInfoVersionEntryDto CreateSystemInfoVersionEntryFromEnv(string envName)
            {
                return new SystemInfoVersionEntryDto(envName, string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envName)) ? "dev" : Environment.GetEnvironmentVariable(envName));
            }
        }

        public class Request : IRequest<Response>
        {
            public Request()
            {
            }
        }

        public class Response : ResponseBase<SystemInfoDto>
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
