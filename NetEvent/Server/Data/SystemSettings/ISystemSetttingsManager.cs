using System.Threading.Tasks;

using NetEvent.Server.Models;

namespace NetEvent.Server.Data.SystemSettings
{
    public interface ISystemSettingsManager
    {
        Task<SystemSettingsResult> UpdateAsync(SystemSettingValue systemSettingValueToUpdate);

    }
}
