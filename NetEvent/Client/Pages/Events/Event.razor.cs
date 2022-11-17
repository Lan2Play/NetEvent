using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages.Events
{
    public partial class Event
    {
        #region Injects

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IEventService _EventService { get; set; } = default!;

        [Inject]
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        #endregion

        [Parameter]
        public string? Slug { get; set; }

        private bool _Loading = true;
        private EventDto? _Event = default!;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            if (string.IsNullOrEmpty(Slug))
            {
                NavigationManager.NavigateTo("/");
            }

            _Event = await _EventService.GetEventAsync(Slug!, cts.Token);
            _Loading = false;
        }

        private static Color GetStateColor(PublishStateDto state)
        {
            switch (state)
            {
                case PublishStateDto.Draft:
                    return Color.Error;
                case PublishStateDto.Preview:
                    return Color.Warning;
                case PublishStateDto.Published:
                    return Color.Transparent;
                default:
                    throw new NotSupportedException($"PublishState {state} is not supported!");
            }
        }
    }
}
