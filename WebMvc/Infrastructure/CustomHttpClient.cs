using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod,
                    authorizationToken);
            }

            //send the request message; need a client (eg: postman)
            var response = await _client.SendAsync(requestMessage); //pushing send button in postman
            return await response.Content.ReadAsStringAsync(); //converting content that we get back into string
                                                               //since SendAsync send HttpResponseMessage, not string

        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod,
                    authorizationToken);
            }
            return await _client.SendAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
                                                      //which url to post to, body of the item, any token, token method- bearer
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item, authorizationToken, authorizationMethod);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item, authorizationToken, authorizationMethod);
        }

        //DoPostPutAsync method will be called from Put as well as from Post
        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method,
            string uri, T item, string authorizationToken, string authorizationMethod)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }
            var requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item),
                                        System.Text.Encoding.UTF8, "application/json");

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod,
                    authorizationToken);
            }
            var response = await _client.SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return response;
        }
    }
}
