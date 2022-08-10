using System.Threading.Tasks;

using NetEvent.Server.Models;

namespace NetEvent.Server.Data.Events
{
    public interface IEventManager
    {
        Task<EventResult> CreateAsync(Event eventToCreate);

        Task<EventResult> DeleteAsync(long eventId);

        Task<EventResult> DeleteAsync(Event eventToDelete);

        Task<EventResult> UpdateAsync(Event eventToUpdate);

        Task<EventResult> ValidateEventAsync(Event eventToValidate);
    }
}
