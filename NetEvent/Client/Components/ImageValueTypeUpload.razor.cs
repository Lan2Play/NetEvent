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
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        private const string ImageUrl = "/api/system/image/";
        private bool Clearing;
        private string DragClass = string.Empty;
        private IBrowserFile? fileName;
        private string? imageFileName;
        private string? imageSrc;
        private bool uploading;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            imageFileName = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSetting.Key, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            if (!string.IsNullOrEmpty(imageFileName))
            {
                imageSrc = $"{ImageUrl}{imageFileName}";
            }
        }

        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            uploading = true;
            fileName = e.File;

            using var cancellationTokenSource = new CancellationTokenSource();
            var uploadResult = await SystemSettingsDataService.UploadSystemImage(e.File, cancellationTokenSource.Token);
            if (uploadResult.Successful)
            {
                imageFileName = uploadResult.ResultData;
                if (!string.IsNullOrEmpty(imageFileName))
                {
                    await SystemSettingsDataService.UpdateSystemSetting(SystemSettingGroup.OrganizationData, new NetEvent.Shared.Dto.SystemSettingValueDto { Key = SystemSetting.Key, Value = imageFileName }, cancellationTokenSource.Token).ConfigureAwait(false);
                    imageSrc = $"{ImageUrl}{imageFileName}";
                }
            }

            ClearDragClass();
            uploading = false;
        }

        private async Task Clear()
        {
            Clearing = true;
            fileName = null;
            ClearDragClass();
            imageFileName = string.Empty;
            using var cancellationTokenSource = new CancellationTokenSource();
            await SystemSettingsDataService.UpdateSystemSetting(SystemSettingGroup.OrganizationData, new NetEvent.Shared.Dto.SystemSettingValueDto { Key = SystemSetting.Key, Value = imageFileName }, cancellationTokenSource.Token).ConfigureAwait(false);
            imageSrc = string.Empty;
            Clearing = false;
        }

        private void SetDragClass()
        {
            DragClass = $"border-dashed";
        }

        private void ClearDragClass()
        {
            DragClass = string.Empty;
        }
    }
}
