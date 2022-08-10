using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Data.Events
{
    public class EventManager : IEventManager
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IServiceProvider _Services;
        private readonly ILogger<EventManager> _Logger;

        public EventManager(ApplicationDbContext dbContext, IServiceProvider services, ILogger<EventManager> logger)
        {
            _DbContext = dbContext;
            _Services = services;
            _Logger = logger;
        }

        protected CancellationToken CancellationToken => CancellationToken.None;

        public Task<EventResult> ValidateEventAsync(Event eventToValidate)
        {
            if (eventToValidate == null)
            {
                return Task.FromResult(EventResult.Failed(new EventError { Description = "Event can not be null!" }));
            }

            // TODO Add More Validation
            return Task.FromResult(EventResult.Success);
        }

        public async Task<EventResult> CreateAsync(Event eventToCreate)
        {
            var addResult = await _DbContext.Events.AddAsync(eventToCreate, CancellationToken);
            if (addResult.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                _Logger.LogInformation("Successfully created Event {name}", eventToCreate.Name);
                return EventResult.Success;
            }

            _Logger.LogError("Error creating Event {name}", eventToCreate.Name);
            return EventResult.Failed(new EventError());
        }

        public async Task<EventResult> UpdateAsync(Event eventToUpdate)
        {
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
            _Logger.LogError("Error deleting Event {name}", eventToDelete.Name);
            return EventResult.Failed(new EventError());
        }
    }
}
