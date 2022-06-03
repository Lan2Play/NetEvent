using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace NetEvent.Client.Components
{
    public class NetEventDataGrid<T> : MudBlazor.MudDataGrid<T>
    {
        [Inject]
        private IDialogService _DialogService { get; set; } = default;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        [Parameter]
        public EventCallback<T> DeletedItemChanges { get; set; }

        public async Task AddNewItemAsync()
        {
            if (!ReadOnly)
            {
                if (Items is not IList list)
                {
                    throw new NotSupportedException($"ItemsSource of Type '{Items.GetType().Name}' is not supported! It has to be of Type 'IList'");
                }

                var newItem = Activator.CreateInstance<T>();

                var oldCommittedItemChanges = CommittedItemChanges;
                CommittedItemChanges = new EventCallbackFactory().Create<T>(this, async d =>
                {
                    await oldCommittedItemChanges.InvokeAsync(d);
                    CommittedItemChanges = oldCommittedItemChanges;
                });
                var oldCancelledEditingItem = CancelledEditingItem;
                CancelledEditingItem = new EventCallbackFactory().Create<T>(this, async d =>
                {
                    list.Remove(newItem);
                    await oldCancelledEditingItem.InvokeAsync(d);
                    CancelledEditingItem = oldCancelledEditingItem;
                });

                list.Add(newItem);
                await SetEditingItemAsync(newItem);
            }
        }

        public async Task DeleteItemAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (Items is not IList list)
            {
                throw new NotSupportedException($"ItemsSource of Type '{Items.GetType().Name}' is not supported! It has to be of Type 'IList'");
            }

            await DeleteItemsInternalAsync(item, list);
        }

        private async Task DeleteItemsInternalAsync(T item, IList list)
        {
            bool? result = await _DialogService.ShowMessageBox(_Localizer.GetString("NetEventDataGrid.DeleteDialog.Title"), _Localizer.GetString("NetEventDataGrid.DeleteDialog.Message"), yesText: _Localizer.GetString("NetEventDataGrid.DeleteDialog.Delete"), cancelText: _Localizer.GetString("NetEventDataGrid.DeleteDialog.Cancel"));
            if (result == true)
            {
                StateHasChanged();

                list.Remove(item);
                await DeletedItemChanges.InvokeAsync(item);
            }
        }
    }
}
