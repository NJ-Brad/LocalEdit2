using LocalEdit2.IPAddressTypes;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Octokit.Internal;
using System.Text.Json;

namespace LocalEdit2.LaunchTypes
{
    // https://dev.to/moe23/blazor-wasm-with-rest-api-step-by-step-2djo
    // https://www.myip.com/api-docs/

    public class LaunchItemDataService : ILaunchItemDataService
    {
        // Using the httpclient to call our API, 
        // its being initialized via the constructor injection
        private readonly HttpClient _httpClient;

        public LaunchItemDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<LaunchItem>> GetAllItems()
        {
            // this is for testing

            // https://stackoverflow.com/questions/64858434/net5-0-blazor-wasm-cors-client-exception
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri($"{_httpClient.BaseAddress}json/launches/next/5")
            };

            request.SetBrowserRequestCache(BrowserRequestCache.NoStore); //optional 

            var apiResponse1 = await _httpClient.SendAsync(request);
            string txt = await apiResponse1.Content.ReadAsStringAsync();
            //var txt = await apiResponse.Content.ReadAsStringAsync();

            var apiResponse = await _httpClient.GetStreamAsync($"json/launches/next/5");
            return await JsonSerializer.DeserializeAsync<IEnumerable<LaunchItem>>
                                (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<LaunchItem> GetItemDetails(int id)
        {
            var apiResponse = await _httpClient.GetStreamAsync($"api/todo/{id}");
            return await JsonSerializer.DeserializeAsync<LaunchItem>
                                (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<AddressInfo> GetIPAddressInfo()
        {
            // https://stackoverflow.com/questions/64858434/net5-0-blazor-wasm-cors-client-exception
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                //RequestUri = new Uri($"{site.BaseUrl}robots.txt")
                RequestUri = new Uri($"{_httpClient.BaseAddress}")
            };

            //            request.Headers.Add("Accept", "text/plain");
            //request.SetBrowserRequestMode(BrowserRequestMode.NoCors);

            //            request.SetBrowserRequestMode(BrowserRequestMode.Cors);
            //            request.Headers.Add("Access-Control-Allow-Origin", _httpClient.BaseAddress.ToString());
            //request.SetBrowserRequestMode(BrowserRequestMode.NoCors);
            request.SetBrowserRequestCache(BrowserRequestCache.NoStore); //optional 

            //            var apiResponse = await _httpClient.GetStreamAsync($"api/todo/{id}");
            //            var apiResponse = await _httpClient.GetStreamAsync("");
            //return await JsonSerializer.DeserializeAsync<AddressInfo>
            //                    (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            // https://stackoverflow.com/questions/47513400/httpclient-getstreamasync-with-custom-request#47513859
            //var request = new HttpRequestMessage(HttpMethod.???, uri);
            //// add Content, Headers, etc to request
            //request.Content = new StringContent(yourJsonString, System.Text.Encoding.UTF8, "application/json");
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            //response.EnsureSuccessStatusCode();
            //var stream = await response.Content.ReadAsStreamAsync();


            var apiResponse = await _httpClient.SendAsync(request);


            var txt = await apiResponse.Content.ReadAsStringAsync();
            //return var txt = await apiResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AddressInfo>
                                (apiResponse.Content.ReadAsStreamAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
