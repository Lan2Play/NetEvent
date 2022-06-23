using System;
using System.Reflection;
using System.Security.Policy;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemInfo
{
    public class GetSystemInfoHandler : IRequestHandler<GetSystemInfoRequest, GetSystemInfoResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetSystemInfoHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public Task<GetSystemInfoResponse> Handle(GetSystemInfoRequest request, CancellationToken cancellationToken)
        {
            var systeminfo = new List<SystemInfoDto>();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            foreach (Assembly assem in assems)
            {
                systeminfo.Add(new SystemInfoDto(assem.ToString(),assem.ToString()));
            }

            return Task.FromResult(new GetSystemInfoResponse(systeminfo));
        }
    }
}
