using Bandydos.Dto.Enums;
using BandydosMobile.Models.Constants;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

//[assembly: Dependency(typeof(SportPlanner.Repository.GenericRepository))]

namespace BandydosMobile.Repository
{
    public class GenericRepository : IGenericRepository
    {
        static private HttpClient _client = new();

        public async Task<T?> GetAsync<T>(string requestUri)
        {
            HttpResponseMessage response = await _client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }

        public async Task<(CrudResult, T?)> PostAsync<T>(string requestUri, T body)
        {
            var content = new StringContent(JsonSerializer.Serialize(body));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PostAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<T>(responseContent);

                return (CrudResult.Ok, result);
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return (CrudResult.AlreadyExists, default);
            }

            return (CrudResult.Error, default);
        }

        public async Task<bool> PutAsync<T>(string requestUri, T body)
        {
            var content = new StringContent(JsonSerializer.Serialize(body));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PutAsync(requestUri, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PatchAsync<T>(string requestUri, T body)
        {
            var method = new HttpMethod("PATCH");
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            var response = await _client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string requestUri)
        {
                HttpResponseMessage response = await _client.DeleteAsync(requestUri);

                return response.IsSuccessStatusCode;
        }

//        private static HttpClient GetHttpClient()
//        {
//#if DEBUG
//            HttpClientHandler insecureHandler = GetInsecureHandler();
//            return CreateHttpClient(insecureHandler);
//#else
//            return CreateHttpClient();
//#endif
//        }

        //private static HttpClient CreateHttpClient(HttpClientHandler httpClientHandler = null)
        //{
        //    var client = httpClientHandler is null
        //        ? new HttpClient()
        //        : new HttpClient(httpClientHandler);
        //    client.DefaultRequestHeaders.Add("x-functions-key", UriConstants.Apikey);

        //    return client;
        //}

        //private static HttpClientHandler GetInsecureHandler()
        //{
        //    return new HttpClientHandler
        //    {
        //        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        //        {
        //            if (cert.Issuer.Equals("CN=localhost"))
        //            {
        //                return true;
        //            }

        //            return errors == System.Net.Security.SslPolicyErrors.None;
        //        }
        //    };
        //}
    }
}
