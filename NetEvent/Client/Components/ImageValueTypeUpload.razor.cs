using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;

namespace NetEvent.Client.Components
{
    public partial class ImageValueTypeUpload
    {
        [Parameter]
        public ImageValueType ImageValueType { get; set; } = default!;

        [Parameter]
        public SystemSetting SystemSetting { get; set; } = default!;

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        private const string ImageUrl = "/api/system/image/";
        private bool Clearing = false;
        private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string DragClass = DefaultDragClass;
        private IBrowserFile? fileName;
        private string? imageFileName;
        private string? imageSrc;
        private bool uploading;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            imageFileName = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSetting.Key, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            if (!string.IsNullOrEmpty(imageFileName))
            {
                imageSrc = $"{ImageUrl}{imageFileName}";
            }
        }

        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            uploading = true;
            ClearDragClass();
            fileName = e.File;

            using var cancellationTokenSource = new CancellationTokenSource();
            var uploadResult = await _SystemSettingsDataService.UploadSystemImage(e.File, cancellationTokenSource.Token);
            if (uploadResult.Successful)
            {
                imageFileName = uploadResult.ResultData;
                if (!string.IsNullOrEmpty(imageFileName))
                {
                    await _SystemSettingsDataService.UpdateSystemSetting(SystemSettingGroup.OrganizationData, new NetEvent.Shared.Dto.SystemSettingValueDto { Key = SystemSetting.Key, Value = imageFileName }, cancellationTokenSource.Token).ConfigureAwait(false);
                    imageSrc = $"{ImageUrl}{imageFileName}";
                }
            }

            uploading = false;
        }

        private async Task Clear()
        {
            Clearing = true;
            fileName = null;
            ClearDragClass();
            await Task.Delay(100);
            Clearing = false;
        }

        private void SetDragClass()
        {
            DragClass = $"{DefaultDragClass} mud-border-primary";
        }

        private void ClearDragClass()
        {
            DragClass = DefaultDragClass;
        }
    }
}
