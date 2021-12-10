using Microsoft.AspNetCore.Components;
using NetEvent.Client.Models;
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

        public List<User> Users { get; private set; } = new List<User>();

        protected override void OnInitialized()
        {
            if (NetEventClient != null)
            {

                _UsersSession = NetEventClient.GetUsers.Watch(ExecutionStrategy.CacheFirst)
                                                          .Where(t => !t.Errors.Any())
                                                          .Select(t => t.Data!.Users)
                                                          .Subscribe(result =>
                                                          {
                                                              Users.AddRange(result.Select(x => x.ToUser()));
                                                              StateHasChanged();
                                                          });

                _UsersSession = NetEventClient.UserAdded.Watch(ExecutionStrategy.CacheFirst)
                                                             .Where(t => !t.Errors.Any())
                                                             .Select(t => t.Data!.UserAdded)
                                                             .Subscribe(result =>
                                                             {
                                                                 Users.Add(result.ToUser());
                                                                 StateHasChanged();
                                                             });
            }
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