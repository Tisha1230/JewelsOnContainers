using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public interface IHttpClient //provide contract that no matter what implementation comes along, it will have these methods defined
    {
        Task<string> GetStringAsync(string uri,
            string authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> PostAsync<T>(string uri, //POST: creating 
            T item,
            string authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> DeleteAsync(string uri,
            string authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> PutAsync<T>(string uri, //PUT: for editing
            T item, string authorizationToken = null,
            string authorizationMethod = "Bearer");
    }
}
