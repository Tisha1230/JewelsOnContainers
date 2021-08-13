using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly HttpClient _client; 
        public CustomHttpClient()
        {
            _client = new HttpClient(); //instance of client = opening instance of postman
        }

        public async Task<string> GetStringAsync(string uri,
            string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri); //for every request need to pass typeOfrequest and URI(from ApiPath)

            //send the request message; need a client (eg: postman)
            var response = await _client.SendAsync(requestMessage); //pushing send button in postman
            return await response.Content.ReadAsStringAsync(); //converting content that we get back into string
                                                               //since SendAsync send HttpResponseMessage, not string

        }

        public Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }
    }
}
