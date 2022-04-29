using MudBlazor.ThemeManager;

namespace NetEvent.Client.Services
{
    public interface IThemeService
    {
        Task<ThemeManagerTheme?> GetThemeAsync(CancellationToken cancellationToken);
        Task<bool> UpdateThemeAsync(ThemeManagerTheme updatedTheme, CancellationToken cancellationToken);
    }
}