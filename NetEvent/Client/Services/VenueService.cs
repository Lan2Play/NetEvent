using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Services
{
    public class VenueService : IVenueService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<VenueService> _Logger;

        public VenueService(IHttpClientFactory httpClientFactory, ILogger<VenueService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<ServiceResult> CreateVenueAsync(VenueDto venueDto, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/venues", venueDto, cancellationToken);

                response.EnsureSuccessStatusCode();

                venueDto.Id = long.Parse(await response.Content.ReadAsStringAsync(cancellationToken), CultureInfo.InvariantCulture);

                return ServiceResult.Success("VenueService.AddVenueAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to create venue in backend.");
            }

            return ServiceResult.Error("VenueService.AddVenueAsync.Error");
        }

        public async Task<List<VenueDto>> GetVenuesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var venues = await client.GetFromJsonAsync<List<VenueDto>>("/api/venues", cancellationToken).ConfigureAwait(false);

                if (venues == null)
                {
                    _Logger.LogError("Unable to get roles data from backend");
                    return new List<VenueDto>();
                }

                return venues;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get roles data from backend");
                return new List<VenueDto>();
            }
        }

        public async Task<VenueDto?> GetVenueAsync(string slug, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var venueDto = await client.GetFromJsonAsync<VenueDto?>($"/api/venues/{slug}", cancellationToken).ConfigureAwait(false);

                if (venueDto == null)
                {
                    _Logger.LogError("Unable to get venue data from backend");
                    return null;
                }

                return venueDto;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get venue data from backend");
                return null;
            }
        }

        public Task<VenueDto?> GetVenueAsync(long id, CancellationToken cancellationToken)
        {
            return GetVenueAsync(id.ToString(CultureInfo.InvariantCulture), cancellationToken);
        }

        public async Task<ServiceResult> UpdateVenueAsync(VenueDto venueDto, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PutAsJsonAsync($"api/venues/{venueDto.Id}", venueDto, cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("VenueService.UpdateVenueAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update venue in backend.");
            }

            return ServiceResult.Error("VenueService.UpdateVenueAsync.Error");
        }

        public async Task<ServiceResult> DeleteVenueAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.DeleteAsync($"api/venues/{id}", cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("VenueService.DeleteVenueAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to delete venue in backend.");
            }

            return ServiceResult.Error("VenueService.DeleteVenueAsync.Error");
        }
    }
}
