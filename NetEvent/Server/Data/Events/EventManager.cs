using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using Slugify;

namespace NetEvent.Server.Data.Events
{
    public class EventManager : IEventManager
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly ILogger<EventManager> _Logger;
        private readonly ISlugHelper _SlugHelper;

        public EventManager(ApplicationDbContext dbContext, ILogger<EventManager> logger, ISlugHelper slugHelper)
        {
            _DbContext = dbContext;
            _Logger = logger;
            _SlugHelper = slugHelper;
        }

        protected CancellationToken CancellationToken => CancellationToken.None;

        public Task<EventResult> ValidateEventAsync(Event eventToValidate)
        {
            if (eventToValidate == null)
            {
                return Task.FromResult(EventResult.Failed(new EventError { Description = "Event can not be null!" }));
            }

            return Task.FromResult(EventResult.Success);
        }

        public async Task<EventResult> CreateAsync(Event eventToCreate)
        {
            var maxId = await _DbContext.Events.MaxAsync(x => x.Id);
            eventToCreate.Id = maxId.HasValue ? maxId.Value + 1 : 1;
            eventToCreate.Slug = _SlugHelper.GenerateSlug(eventToCreate.Name);
            eventToCreate.Venue = null;

            var validationResult = await ValidateEventAsync(eventToCreate);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            var addResult = await _DbContext.Events.AddAsync(eventToCreate, CancellationToken);
            if (addResult.State == EntityState.Added)
            {
                await _DbContext.SaveChangesAsync();
                _Logger.LogInformation("Successfully created Event {name}", eventToCreate.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error creating Event {name}", eventToCreate.Name);
            return EventResult.Failed(new EventError());
        }

        public async Task<EventResult> UpdateAsync(Event eventToUpdate)
        {
            eventToUpdate.Slug = _SlugHelper.GenerateSlug(eventToUpdate.Name);
            var result = _DbContext.Events.Update(eventToUpdate);

            if (result.State == EntityState.Modified)
            {
                await _DbContext.SaveChangesAsync();
                _Logger.LogInformation("Successfully updated Event {name}", eventToUpdate.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error updating Event {name}", eventToUpdate.Name);
            return EventResult.Failed(new EventError());
        }

        public async Task<EventResult> DeleteAsync(long eventId)
        {
            var eventToDelete = await _DbContext.Events.FindAsync(eventId);
            if (eventToDelete == null)
            {
                return EventResult.Failed(new EventError { Description = $"Event with Id '{eventId}' was not found" });
            }

            return await DeleteAsync(eventToDelete);
        }

        public async Task<EventResult> DeleteAsync(Event eventToDelete)
        {
            var existingEvent = await _DbContext.Events.FindAsync(eventToDelete.Id);
            if (existingEvent == null)
            {
                return EventResult.Failed(new EventError { Description = $"Event with Id '{eventToDelete.Id}' was not found" });
            }

            var result = _DbContext.Events.Remove(eventToDelete);

            if (result.State == EntityState.Deleted)
            {
                await _DbContext.SaveChangesAsync();
                _Logger.LogInformation("Successfully deleted Event {name}", eventToDelete.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error deleting Event {name}", eventToDelete.Name);
            return EventResult.Failed(new EventError());
        }

        public async Task<EventResult> DeleteVenueAsync(long venueId)
        {
            var eventToDelete = await _DbContext.Venues.FindAsync(venueId);
            if (eventToDelete == null)
            {
                return EventResult.Failed(new EventError { Description = $"Venue with Id '{venueId}' was not found" });
            }

            _DbContext.Venues.Remove(eventToDelete);
            await _DbContext.SaveChangesAsync();

            return EventResult.Success;
        }

        public async Task<EventResult> CreateVenueAsync(Venue venueToCreate)
        {
            var maxId = await _DbContext.Venues.MaxAsync(x => x.Id);
            venueToCreate.Id = maxId.HasValue ? maxId.Value + 1 : 1;
            venueToCreate.Slug = _SlugHelper.GenerateSlug(venueToCreate.Name);

            var addResult = await _DbContext.Venues.AddAsync(venueToCreate, CancellationToken);
            if (addResult.State == EntityState.Added)
            {
                await _DbContext.SaveChangesAsync();
                _Logger.LogInformation("Successfully created Venue {name}", venueToCreate.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error creating Venue {name}", venueToCreate.Name);
            return EventResult.Failed(new EventError());
        }

        public async Task<EventResult> UpdateVenueAsync(Venue venueToUpdate)
        {
            venueToUpdate.Slug = _SlugHelper.GenerateSlug(venueToUpdate.Name);
            var result = _DbContext.Venues.Update(venueToUpdate);

            if (result.State == EntityState.Modified)
            {
                await _DbContext.SaveChangesAsync();
                _Logger.LogInformation("Successfully updated Venue {name}", venueToUpdate.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error updating Venue {name}", venueToUpdate.Name);
            return EventResult.Failed(new EventError());
        }

        public async Task<EventResult> CreateTicketAsync(EventTicketType ticketToCreate)
        {
            var maxId = await _DbContext.Tickets.MaxAsync(x => x.Id);
            ticketToCreate.Id = maxId.HasValue ? maxId.Value + 1 : 1;
            ticketToCreate.Slug = _SlugHelper.GenerateSlug(ticketToCreate.Name);

            var addResult = await _DbContext.Tickets.AddAsync(ticketToCreate, CancellationToken);
            if (addResult.State == EntityState.Added)
            {
                await _DbContext.SaveChangesAsync();
                _Logger.LogInformation("Successfully created Ticket {name}", ticketToCreate.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error creating Ticket {name}", ticketToCreate.Name);
            return EventResult.Failed(new EventError());
        }

        public async Task<EventResult> DeleteTicketAsync(long ticketId)
        {
            var ticketToDelete = await _DbContext.Tickets.FindAsync(ticketId);
            if (ticketToDelete == null)
            {
                return EventResult.Failed(new EventError { Description = $"Ticket with Id '{ticketId}' was not found" });
            }

            _DbContext.Tickets.Remove(ticketToDelete);
            await _DbContext.SaveChangesAsync();

            return EventResult.Success;
        }

        public async Task<EventResult> UpdateTicketAsync(EventTicketType ticketToUpdate)
        {
            var oldTicketType = await _DbContext.Tickets.Where(t => t.Id == ticketToUpdate.Id).Include(t => t.Event).FirstAsync(CancellationToken).ConfigureAwait(false);
            if (oldTicketType == null)
            {
                _Logger.LogError("Error updating Ticket {name} with {id}. It was not found.", ticketToUpdate.Name, ticketToUpdate.Id);
                return EventResult.Failed(new EventError());
            }

            ticketToUpdate.Slug = _SlugHelper.GenerateSlug(ticketToUpdate.Name);
            ticketToUpdate.EventId = oldTicketType.EventId;
            ticketToUpdate.Event = oldTicketType.Event;
            _DbContext.Entry(oldTicketType).State = EntityState.Detached;

            var result = _DbContext.Tickets.Update(ticketToUpdate);

            if (result.State == EntityState.Modified)
            {
                await _DbContext.SaveChangesAsync();
                _Logger.LogInformation("Successfully updated Ticket {name}", ticketToUpdate.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error updating Ticket {name}", ticketToUpdate.Name);
            return EventResult.Failed(new EventError());
        }
    }
}
