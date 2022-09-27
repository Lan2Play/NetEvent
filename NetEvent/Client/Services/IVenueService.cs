using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Services
{
    public interface IVenueService
    {
        Task<List<VenueDto>> GetVenuesAsync(CancellationToken cancellationToken);

        Task<VenueDto?> GetVenueAsync(long id, CancellationToken cancellationToken);
        
        Task<VenueDto?> GetVenueAsync(string slug, CancellationToken cancellationToken);

        Task<ServiceResult> DeleteVenueAsync(long id, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateVenueAsync(VenueDto venueDto, CancellationToken cancellationToken);

        Task<ServiceResult> CreateVenueAsync(VenueDto venueDto, CancellationToken cancellationToken);
    }
}
