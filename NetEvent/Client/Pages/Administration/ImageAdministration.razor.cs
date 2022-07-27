using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class ImageAdministration
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        #endregion

        private const string ImageUrl = "/api/system/image/";
        private List<SystemImageWithUsagesDto> Images = new();

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            Images = await _SystemSettingsDataService.GetSystemImagesAsync(cancellationTokenSource.Token);
        }

        private static string GetImageUrl(string imageId) => $"{ImageUrl}{imageId}";

        private async Task DeleteImage(SystemImageWithUsagesDto image)
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var result = await _SystemSettingsDataService.DeleteSystemImage(image.Image.Id!, cancellationTokenSource.Token);
            if (result.Successful)
            {
                Images.Remove(image);
            }
        }
    }
}
