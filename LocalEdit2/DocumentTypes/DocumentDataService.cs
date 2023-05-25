using LocalEdit2.IPAddressTypes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Text.Json;


namespace LocalEdit2.DocumentTypes
{
    // https://dev.to/moe23/blazor-wasm-with-rest-api-step-by-step-2djo
    // https://www.myip.com/api-docs/
    public class DocumentDataService : IDocumentDataService
    {
        // Using the httpclient to call our API, 
        // its being initialized via the constructor injection
        private readonly HttpClient _httpClient;

        public DocumentDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Document>> GetAllDocuments()
        {

            // https://stackoverflow.com/questions/64858434/net5-0-blazor-wasm-cors-client-exception
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri($"{_httpClient.BaseAddress}api/document")
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

            var apiResponse = await _httpClient.GetStreamAsync($"api/document");
            return await JsonSerializer.DeserializeAsync<IEnumerable<Document>>
                                (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Document> GetDocument(string id)
        {
            //var request = new HttpRequestMessage
            //{
            //    Method = new HttpMethod("GET"),
            //    RequestUri = new Uri($"{_httpClient.BaseAddress}api/document/{id}")
            //};

            //request.SetBrowserRequestCache(BrowserRequestCache.NoStore); //optional 

            //try
            //{
            //    var apiResponse1 = await _httpClient.SendAsync(request);

            //    string txt = await apiResponse1.Content.ReadAsStringAsync();
            //    var v2 =JsonSerializer.Deserialize<IEnumerable<Document>>
            //                    (txt, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //    var v3 = v2.ToArray<Document>()[0];
            //}
            //catch (Exception ex)
            //{
            //    System.Console.WriteLine(ex.ToString());
            //}

            var apiResponse = await _httpClient.GetStreamAsync($"api/document/{id}");
            var responseData = await JsonSerializer.DeserializeAsync<IEnumerable<Document>>
                                (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var arrayData = responseData.ToArray<Document>();

            Document rtnVal = null;
            if((arrayData!= null) && (arrayData.Length>0))
            {
                rtnVal = arrayData[0];
            }

            return rtnVal;
        }
        public async Task<Document> DeleteDocument(string id)
        {
            var apiResponse = await _httpClient.GetStreamAsync($"api/todo/{id}");
            return await JsonSerializer.DeserializeAsync<Document>
                                (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Document> UpdateDocument(Document document)
        {
            try
            {
                document.Modified = DateTime.Now;

                // https://stackoverflow.com/questions/4015324/send-http-post-request-in-net
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/document/{document.Id}");
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(document), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                //                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Do something here

                    // The location header is not sent through.  Probably a CORS issue.  I decided to include the new document ID as the content
                    string txt = await response.Content.ReadAsStringAsync();
                    document.Id = txt;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            ////var apiResponse = await _httpClient.GetStreamAsync($"api/todo/{plan.id}");
            //var apiResponse = await _httpClient.GetStreamAsync($"api/document");
            //return await JsonSerializer.DeserializeAsync<Document>
            //                    (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return document;
        }
        public async Task<Document> CreateDocument(Document document)
        {
            // POST is Create for this project
            // need to see how to get the new ID (it is in the location header)

            try
            {
                // https://stackoverflow.com/questions/4015324/send-http-post-request-in-net
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/document");
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(document), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
//                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Do something here

                    // The location header is not sent through.  Probably a CORS issue.  I decided to include the new document ID as the content
                    string txt = await response.Content.ReadAsStringAsync();
                    document.Id = txt;
                }
                else
                {
                    string txt = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            ////var apiResponse = await _httpClient.GetStreamAsync($"api/todo/{plan.id}");
            //var apiResponse = await _httpClient.GetStreamAsync($"api/document");
            //return await JsonSerializer.DeserializeAsync<Document>
            //                    (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return document;
        }

        //public async Task<AddressInfo> GetIPAddressInfo()
        //{
        //    // https://stackoverflow.com/questions/64858434/net5-0-blazor-wasm-cors-client-exception
        //    var request = new HttpRequestMessage
        //    {
        //        Method = new HttpMethod("GET"),
        //        //RequestUri = new Uri($"{site.BaseUrl}robots.txt")
        //        RequestUri = new Uri($"{_httpClient.BaseAddress}")
        //    };

        //    //            request.Headers.Add("Accept", "text/plain");
        //    //request.SetBrowserRequestMode(BrowserRequestMode.NoCors);

        //    //            request.SetBrowserRequestMode(BrowserRequestMode.Cors);
        //    //            request.Headers.Add("Access-Control-Allow-Origin", _httpClient.BaseAddress.ToString());
        //    //request.SetBrowserRequestMode(BrowserRequestMode.NoCors);
        //    request.SetBrowserRequestCache(BrowserRequestCache.NoStore); //optional 

        //    //            var apiResponse = await _httpClient.GetStreamAsync($"api/todo/{id}");
        //    //            var apiResponse = await _httpClient.GetStreamAsync("");
        //    //return await JsonSerializer.DeserializeAsync<AddressInfo>
        //    //                    (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        //    // https://stackoverflow.com/questions/47513400/httpclient-getstreamasync-with-custom-request#47513859
        //    //var request = new HttpRequestMessage(HttpMethod.???, uri);
        //    //// add Content, Headers, etc to request
        //    //request.Content = new StringContent(yourJsonString, System.Text.Encoding.UTF8, "application/json");
        //    //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        //    //response.EnsureSuccessStatusCode();
        //    //var stream = await response.Content.ReadAsStreamAsync();


        //    var apiResponse = await _httpClient.SendAsync(request);


        //    var txt = await apiResponse.Content.ReadAsStringAsync();
        //    //return var txt = await apiResponse.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<AddressInfo>
        //                        (apiResponse.Content.ReadAsStreamAsync().Result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        //}
    }
}
