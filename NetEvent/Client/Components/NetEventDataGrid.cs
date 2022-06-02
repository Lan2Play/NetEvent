using System;
using System.Threading.Tasks;

namespace NetEvent.Client.Components
{
    public class NetEventDataGrid<T> : MudBlazor.MudDataGrid<T>
    {
        public async Task AddNewItemAsync()
        {
            if (!ReadOnly)
            {
                var newItem = Activator.CreateInstance<T>();
                await SetEditingItemAsync(newItem);
            }
        }
    }
}
