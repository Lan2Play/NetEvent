using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
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
        private IEventService EventService { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Inject]
        private ICartService CartService { get; set; } = default!;

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

            _Event = await EventService.GetEventAsync(Slug!, cts.Token);
            _Loading = false;
        }

        private static Color GetStateColor(PublishStateDto state)
        {
            return state switch
            {
                PublishStateDto.Draft => Color.Error,
                PublishStateDto.Preview => Color.Warning,
                PublishStateDto.Published => Color.Transparent,
                _ => throw new($"PublishState {state} is not supported!"),
            };
        }

        private async Task BuyTicketAsync(EventTicketTypeDto eventTicketType)
        {
            // /checkout/ticket/{tickettypeid}/{count}
            NavigationManager.NavigateTo($"checkout/ticket/{eventTicketType.Id}");

            //CartService.AddToCart(eventTicketType);
        }
    }
}
