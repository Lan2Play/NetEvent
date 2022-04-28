using NetEvent.Shared.Dto;
using System.Net.Http.Json;

namespace NetEvent.Client.Services
{
    public class OrganizationDataService 
    {
        private const string _HttpClientName = "NetEvent.ServerAPI";

        private readonly IHttpClientFactory _HttpClientFactory;
        public OrganizationDataService(IHttpClientFactory httpClientFactory)
        {
            _HttpClientFactory = httpClientFactory;
        }
        public async Task<List<OrganizationDataDto>> GetOrganizationData()
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(_HttpClientName);

                var result = await client.GetFromJsonAsync<List<OrganizationDataDto>>("api/organization/all");
                return result;
            }
            catch (Exception ex)
            {

                return new List<OrganizationDataDto>() {};
            }
        }

    }
}
