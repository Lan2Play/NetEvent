using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemInfo
{
    public class GetSystemInfoHandler : IRequestHandler<GetSystemInfoRequest, GetSystemInfoResponse>
    {
        public GetSystemInfoHandler()
        {
        }

        public Task<GetSystemInfoResponse> Handle(GetSystemInfoRequest request, CancellationToken cancellationToken)
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

            // TODO: is it possible to make that better?
            systeminfoversions.Add(new SystemInfoVersionEntryDto("NETEVENT", Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion));
            systeminfoversions.Add(new SystemInfoVersionEntryDto("BUILDNODE", string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILDNODE")) ? "dev" : Environment.GetEnvironmentVariable("BUILDNODE")));
            systeminfoversions.Add(new SystemInfoVersionEntryDto("BUILDID", string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILDID")) ? "dev" : Environment.GetEnvironmentVariable("BUILDNODE")));
            systeminfoversions.Add(new SystemInfoVersionEntryDto("BUILDNUMBER", string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILDNUMBER")) ? "dev" : Environment.GetEnvironmentVariable("BUILDNODE")));
            systeminfoversions.Add(new SystemInfoVersionEntryDto("SOURCE_COMMIT", string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SOURCE_COMMIT")) ? "dev" : Environment.GetEnvironmentVariable("BUILDNODE")));

            // TODO: think about healthchecks and healthcheck modularity (to perform checks on various services like game servers, the mail server, payment apis ...) and remove dummy services
            systeminfohealth.Add(new SystemInfoHealthEntryDto("NETEVENT Server", string.Empty, true));
            systeminfohealth.Add(new SystemInfoHealthEntryDto("Email Service", "servername", false));

            var systeminfo = new SystemInfoDto(systeminfocomponents, systeminfohealth, systeminfoversions);

            return Task.FromResult(new GetSystemInfoResponse(systeminfo));
        }
    }
}
