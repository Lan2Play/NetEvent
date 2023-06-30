using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Services
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Services")]
    public class EventService : IEventService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<EventService> _Logger;

        public EventService(IHttpClientFactory httpClientFactory, ILogger<EventService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<ServiceResult> CreateEventAsync(EventDto eventDto, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/events", eventDto, cancellationToken);

                response.EnsureSuccessStatusCode();

                eventDto.Id = long.Parse(await response.Content.ReadAsStringAsync(cancellationToken), CultureInfo.InvariantCulture);

                return ServiceResult.Success("EventService.AddAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to create event in backend.");
            }

            return ServiceResult.Error("EventService.AddAsync.Error");
        }

        public async Task<EventDto?> GetEventAsync(string slug, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var eventDto = await client.GetFromJsonAsync<EventDto?>($"/api/events/{slug}", cancellationToken).ConfigureAwait(false);

                if (eventDto == null)
                {
                    _Logger.LogError("Unable to get event data from backend");
                    return null;
                }

                return eventDto;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get event data from backend");
                return null;
            }
        }

        public Task<EventDto?> GetEventAsync(long id, CancellationToken cancellationToken)
        {
            return GetEventAsync(id.ToString(CultureInfo.InvariantCulture), cancellationToken);
        }

        public async Task<EventDto?> GetUpcomingEventAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var eventDto = await client.GetFromJsonAsync<EventDto?>($"/api/events/upcoming", cancellationToken).ConfigureAwait(false);

                if (eventDto == null)
                {
                    return null;
                }

                return eventDto;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get event data from backend");
                return null;
            }
        }

        public async Task<List<EventDto>> GetEventsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var events = await client.GetFromJsonAsync<List<EventDto>>("/api/events", cancellationToken).ConfigureAwait(false);

                if (events == null)
                {
                    _Logger.LogError("Unable to get roles data from backend");
                    return new List<EventDto>();
                }

                return events;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get roles data from backend");
                return new List<EventDto>();
            }
        }

        public async Task<ServiceResult> UpdateEventAsync(EventDto eventDto, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PutAsJsonAsync($"api/events/{eventDto.Id}", eventDto, cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("EventService.UpdateAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update event in backend.");
            }

            return ServiceResult.Error("EventService.UpdateAsync.Error");
        }

        public async Task<ServiceResult> DeleteEventAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.DeleteAsync($"api/events/{id}", cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("EventService.DeleteAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to delete event in backend.");
            }

            return ServiceResult.Error("EventService.DeleteAsync.Error");
        }

        public async Task<ServiceResult> CreateEventTicketTypeAsync(long eventId, EventTicketTypeDto eventTicketTypeDto, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/events/tickettype/{eventId}", eventTicketTypeDto, cancellationToken);
                response.EnsureSuccessStatusCode();

                eventTicketTypeDto.Id = long.Parse(await response.Content.ReadAsStringAsync(cancellationToken), CultureInfo.InvariantCulture);

                return ServiceResult.Success("EventService.AddAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to create eventTicketType in backend.");
            }

            return ServiceResult.Error("EventService.AddAsync.Error");
        }

        public async Task<ServiceResult> UpdateEventTicketTypeAsync(EventTicketTypeDto eventTicketTypeDto, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PutAsJsonAsync($"api/events/tickettype/{eventTicketTypeDto.Id}", eventTicketTypeDto, cancellationToken);
                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("EventService.UpdateAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update eventTicketType in backend.");
            }

            return ServiceResult.Error("EventService.UpdateAsync.Error");
        }

        public async Task<ServiceResult> DeleteEventTicketTypeAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.DeleteAsync($"api/events/tickettype/{id}", cancellationToken);
                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("EventService.DeleteAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to delete eventTicketType in backend.");
            }

            return ServiceResult.Error("EventService.DeleteAsync.Error");
        }

        public async Task<EventTicketTypeDto?> GetEventTicketTypeAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var eventTicketTypeDto = await client.GetFromJsonAsync<EventTicketTypeDto?>($"/api/events/tickettype/{id}", cancellationToken).ConfigureAwait(false);

                if (eventTicketTypeDto == null)
                {
                    _Logger.LogError("Unable to get eventtickettype data from backend");
                    return null;
                }

                return eventTicketTypeDto;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get eventtickettype data from backend");
                return null;
            }
        }
     }
}
