using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using System.Globalization;
using Slugify;

namespace NetEvent.Server.Data.SystemSettings
{
    public class SystemSettingsManager : ISystemSettingsManager
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly ILogger<SystemSettingsManager> _Logger;
        private readonly ISlugHelper _SlugHelper;

        public SystemSettingsManager(ApplicationDbContext dbContext, ILogger<SystemSettingsManager> logger, ISlugHelper slugHelper)
        {
            _DbContext = dbContext;
            _Logger = logger;
            _SlugHelper = slugHelper;
        }

        protected CancellationToken CancellationToken => CancellationToken.None;


        public async Task<SystemSettingsResult> UpdateAsync(SystemSettingValue systemSettingValueToUpdate)
        {

                if (systemSettingValueToUpdate?.Key == null)
                {
                    _Logger.LogError("Empty key is not allowed");
                    return SystemSettingsResult.Failed(new SystemSettingsError { Description = "Empty key is not allowed" });
                }


                var result = _DbContext.SystemSettingValues.Update(systemSettingValueToUpdate);
          
                if (result.State == EntityState.Modified)
                {
                    await _DbContext.SaveChangesAsync();
                    CheckForCultureChange(systemSettingValueToUpdate);
                    _Logger.LogInformation("Successfully updated Systemsetting {name}", systemSettingValueToUpdate.Key);
                    return SystemSettingsResult.Success;
                }

                _Logger.LogError("Error updating Systemsetting {name}", systemSettingValueToUpdate.Key);
                return SystemSettingsResult.Failed(new SystemSettingsError());


        }


        private void CheckForCultureChange(SystemSettingValue systemSettingValueToUpdate)
        {
            if (systemSettingValueToUpdate?.Key == NetEvent.Shared.Config.SystemSettings.OrganizationData.DataCultureInfo && systemSettingValueToUpdate.SerializedValue != null)
            {
                var cultureInfo = new CultureInfo(systemSettingValueToUpdate.SerializedValue);
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
        }
        
    }
}
