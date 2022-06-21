using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemSettings
{
    public class GetSystemSettingsHandler : IRequestHandler<GetSystemSettingsRequest, GetSystemSettingsResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetSystemSettingsHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public Task<GetSystemSettingsResponse> Handle(GetSystemSettingsRequest request, CancellationToken cancellationToken)
        {
            var organizationData = _ApplicationDbContext.Set<SystemSettingValue>().Select(x => DtoMapper.Mapper.SystemSettingValueToSystemSettingValueDto(x)).ToList();
            return Task.FromResult(new GetSystemSettingsResponse(organizationData));
        }
    }
}
