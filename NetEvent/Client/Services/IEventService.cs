using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Services
{
    public interface IEventService
    {
        Task<List<EventDto>> GetEventsAsync(CancellationToken cancellationToken);

        Task<EventDto?> GetEventAsync(long id, CancellationToken cancellationToken);

        Task<EventDto?> GetEventAsync(string slug, CancellationToken cancellationToken);

        Task<EventDto?> GetUpcomingEventAsync(CancellationToken cancellationToken);

        Task<ServiceResult> DeleteEventAsync(long id, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateEventAsync(EventDto eventDto, CancellationToken cancellationToken);

        Task<ServiceResult> CreateEventAsync(EventDto eventDto, CancellationToken cancellationToken);

        Task<EventTicketTypeDto?> GetEventTicketTypeAsync(long id, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateEventTicketTypeAsync(EventTicketTypeDto eventTicketTypeDto, CancellationToken cancellationToken);

        Task<ServiceResult> CreateEventTicketTypeAsync(long eventId, EventTicketTypeDto eventTicketTypeDto, CancellationToken cancellationToken);
    }
}
