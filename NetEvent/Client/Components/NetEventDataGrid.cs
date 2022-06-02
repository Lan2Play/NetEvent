using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace NetEvent.Client.Components
{
    public class NetEventDataGrid<T> : MudBlazor.MudDataGrid<T>
    {
        public NetEventDataGrid()
        {
        }

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
    }
}
