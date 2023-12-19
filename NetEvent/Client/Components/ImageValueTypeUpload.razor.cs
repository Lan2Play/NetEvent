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
        private const string ImageUrl = "/api/system/image/";
        private bool _Clearing;
        private string _DragClass = string.Empty;
        private IBrowserFile? _FileName;
        private string? _ImageFileName;
        private string? _ImageSrc;
        private bool _Uploading;

        [Parameter]
        public ImageValueType ImageValueType { get; set; } = default!;

        [Parameter]
        public SystemSetting SystemSetting { get; set; } = default!;

        [Inject]
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            _ImageFileName = (await SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSetting.Key, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            if (!string.IsNullOrEmpty(_ImageFileName))
            {
                _ImageSrc = $"{ImageUrl}{_ImageFileName}";
            }
        }

        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            _Uploading = true;
            _FileName = e.File;

            using var cancellationTokenSource = new CancellationTokenSource();
            var uploadResult = await SystemSettingsDataService.UploadSystemImage(e.File, cancellationTokenSource.Token);
            if (uploadResult.Successful)
            {
                _ImageFileName = uploadResult.ResultData;
                if (!string.IsNullOrEmpty(_ImageFileName))
                {
                    await SystemSettingsDataService.UpdateSystemSetting(SystemSettingGroup.OrganizationData, new NetEvent.Shared.Dto.SystemSettingValueDto { Key = SystemSetting.Key, Value = _ImageFileName }, cancellationTokenSource.Token).ConfigureAwait(false);
                    _ImageSrc = $"{ImageUrl}{_ImageFileName}";
                }
            }

            ClearDragClass();
            _Uploading = false;
        }

        private async Task Clear()
        {
            _Clearing = true;
            _FileName = null;
            ClearDragClass();
            _ImageFileName = string.Empty;
            using var cancellationTokenSource = new CancellationTokenSource();
            await SystemSettingsDataService.UpdateSystemSetting(SystemSettingGroup.OrganizationData, new NetEvent.Shared.Dto.SystemSettingValueDto { Key = SystemSetting.Key, Value = _ImageFileName }, cancellationTokenSource.Token).ConfigureAwait(false);
            _ImageSrc = string.Empty;
            _Clearing = false;
        }

        private void SetDragClass()
        {
            _DragClass = $"border-dashed";
        }

        private void ClearDragClass()
        {
            _DragClass = string.Empty;
        }
    }
}
