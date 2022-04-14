namespace NetEvent.Client.Pages.Administration
{
    public partial class Index : IDisposable
    {
        private bool disposedValue;


        protected override void OnInitialized()
        {
       
        }

        #region IDispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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