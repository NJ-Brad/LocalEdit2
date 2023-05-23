using LocalEdit2.IPAddressTypes;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json;

namespace LocalEdit2.DocumentTypes
{
    // https://dev.to/moe23/blazor-wasm-with-rest-api-step-by-step-2djo
    // https://www.myip.com/api-docs/
    public class DocumentIndexDataService : IDocumentIndexDataService
    {
        // Using the httpclient to call our API, 
        // its being initialized via the constructor injection
        private readonly HttpClient _httpClient;

        public DocumentIndexDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Document>> GetAllDocuments(string? DocumentType)
        {
            // https://stackoverflow.com/questions/64858434/net5-0-blazor-wasm-cors-client-exception
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri($"{_httpClient.BaseAddress}api/DocumentIndex/{DocumentType}")
            };

            request.SetBrowserRequestCache(BrowserRequestCache.NoStore); //optional 

            try
            {
                var apiResponse1 = await _httpClient.SendAsync(request);

                string txt = await apiResponse1.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
            //var txt = await apiResponse.Content.ReadAsStringAsync();

            var apiResponse = await _httpClient.GetStreamAsync($"api/DocumentIndex/{DocumentType}");
            return await JsonSerializer.DeserializeAsync<IEnumerable<Document>>
                                (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
