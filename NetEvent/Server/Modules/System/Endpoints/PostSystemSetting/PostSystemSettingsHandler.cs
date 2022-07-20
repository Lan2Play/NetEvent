using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.System.Endpoints.PostSystemSetting
{
    public class PostSystemSettingsHandler : IRequestHandler<PostSystemSettingsRequest, PostSystemSettingsResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public PostSystemSettingsHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public async Task<PostSystemSettingsResponse> Handle(PostSystemSettingsRequest request, CancellationToken cancellationToken)
        {
            if (request.OrganizationData?.Key == null || request.OrganizationData?.Value == null)
            {
                return new PostSystemSettingsResponse(ReturnType.Error, "Empty data is not allowed");
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

            return new PostSystemSettingsResponse();
        }
    }
}
