using Microsoft.AspNetCore.Components;
using NetEvent.Client.GraphQL;
using StrawberryShake;
using System.Reactive.Linq;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index : IDisposable
    {
        private IDisposable? _UsersSession;
        private bool disposedValue;

        [Inject]
        public NetEventClient? NetEventClient { get; set; }

        public IReadOnlyList<IGetUsers_Users> Users { get; private set; } = new List<IGetUsers_Users>();

        protected override void OnInitialized()
        {
            _UsersSession = NetEventClient.GetUsers.Watch(ExecutionStrategy.CacheFirst)
                                                      .Where(t => !t.Errors.Any())
                                                      .Select(t => t.Data!.Users)
                                                      .Subscribe(result =>
                                                      {
                                                          Users = result;
                                                          StateHasChanged();
                                                      });
        }

        #region IDispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _UsersSession.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}