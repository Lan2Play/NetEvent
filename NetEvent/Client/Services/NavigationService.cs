using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace NetEvent.Client.Services
{
    public sealed class NavigationService : IDisposable
    {
        private const int MinHistorySize = 256;
        private const int AdditionalHistorySize = 64;

        private readonly NavigationManager _NavigationManager;
        private readonly List<string> _History;

        public NavigationService(NavigationManager navigationManager)
        {
            _NavigationManager = navigationManager;
            _History = new List<string>(MinHistorySize + AdditionalHistorySize)
            {
                _NavigationManager.Uri
            };
            _NavigationManager.LocationChanged += OnLocationChanged;
        }

        /// <summary>
        /// Navigates to the specified url.
        /// </summary>
        /// <param name="url">The destination url (relative or absolute).</param>
        public void NavigateTo(string url)
        {
            _NavigationManager.NavigateTo(url);
        }

        /// <summary>
        /// Returns true if it is possible to navigate to the previous url.
        /// </summary>
        public bool CanNavigateBack => _History.Count >= 2;

        /// <summary>
        /// Navigates to the previous url if possible or does nothing if it is not.
        /// </summary>
        public void NavigateBack()
        {
            if (!CanNavigateBack)
            {
                return;
            }

            var backPageUrl = _History[^2];
            _History.RemoveRange(_History.Count - 2, 2);
            _NavigationManager.NavigateTo(backPageUrl);
        }

        // .. All other navigation methods.

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            EnsureSize();
            _History.Add(e.Location);
        }

        private void EnsureSize()
        {
            if (_History.Count < MinHistorySize + AdditionalHistorySize)
            {
                return;
            }

            _History.RemoveRange(0, _History.Count - MinHistorySize);
        }

        #region Implementation of Dispose

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _NavigationManager.LocationChanged -= OnLocationChanged;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
