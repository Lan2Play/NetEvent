﻿using System;
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
        private readonly IServiceProvider _Services;
        private readonly ILogger<EventManager> _Logger;
        private readonly ISlugHelper _SlugHelper;

        public EventManager(ApplicationDbContext dbContext, IServiceProvider services, ILogger<EventManager> logger, ISlugHelper slugHelper)
        {
            _DbContext = dbContext;
            _Services = services;
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

            // TODO Add More Validation
            return Task.FromResult(EventResult.Success);
        }

        public async Task<EventResult> CreateAsync(Event eventToCreate)
        {
            var maxId = await _DbContext.Events.MaxAsync(x => x.Id);
            eventToCreate.Id = maxId.HasValue ? maxId.Value + 1 : 1;
            eventToCreate.Slug = _SlugHelper.GenerateSlug(eventToCreate.Name);

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

            await _DbContext.SaveChangesAsync();
            return await DeleteAsync(eventToDelete);
        }

        public async Task<EventResult> DeleteAsync(Event eventToDelete)
        {
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
    }
}