using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace NetEvent.Client.Components
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Controls")]
    public class NetEventDataGrid<T> : MudDataGrid<T>
    {
        [Inject]
        private IDialogService DialogService { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Parameter]
        public EventCallback<EventCallbackArgs<T>> DeletedItemChanges { get; set; }

        public async Task AddNewItemAsync()
        {
            if (ReadOnly)
            {
                return;
            }

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

        public Task DeleteItemAsync(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteItemsInternalAsync(item);
        }

        private async Task DeleteItemsInternalAsync(T item)
        {
            if (Items is not IList list)
            {
                throw new NotSupportedException($"ItemsSource of Type '{Items.GetType().Name}' is not supported! It has to be of Type 'IList'");
            }

            if (ReadOnly)
            {
                return;
            }

            bool? result = await DialogService.ShowMessageBox(Localizer.GetString("NetEventDataGrid.DeleteDialog.Title"), Localizer.GetString("NetEventDataGrid.DeleteDialog.Message"), yesText: Localizer.GetString("NetEventDataGrid.DeleteDialog.Delete"), cancelText: Localizer.GetString("NetEventDataGrid.DeleteDialog.Cancel"));
            if (result == true)
            {
                StateHasChanged();

                var eventArgs = new EventCallbackArgs<T>(item);
                await DeletedItemChanges.InvokeAsync(eventArgs);
                if (!eventArgs.Cancel)
                {
                    list.Remove(item);
                }
            }
        }
    }
}
