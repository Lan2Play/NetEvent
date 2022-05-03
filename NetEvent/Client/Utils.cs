using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetEvent.Client
{
    public static class Utils
    {
        public static async Task<T> Get<T>(this HttpClient httpClient, string apiMethod)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiMethod);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "NetEvent.Client");

                //var client = clientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStreamAsync();
                    var result = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (result != null)
                    {
                        return result;
                    }
                }
                else
                {
                    // TODO ErrorHandling???
                }
            }
            catch (Exception e)
            {

            }
            return default;
        }

        public static async Task Put<T>(this HttpClient httpClient, string apiMethod, T putData)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, apiMethod);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "NetEvent.Client");

                var json = JsonSerializer.Serialize(putData);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                //var client = clientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStreamAsync();
                    var result = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (result != null)
                    {
                        return;
                    }

                    //using var responseStream = await response.Content.ReadAsStreamAsync();
                    //var result =  await JsonSerializer.DeserializeAsync<T>(responseStream);
                    //if (result != null)
                    //{
                    //    return result;
                    //}
                }
                else
                {
                    // TODO ErrorHandling???
                }
            }
            catch (Exception e)
            {

            }

            throw new NotImplementedException("Error Handling is not yet implemented!");
        }
    }
}
