using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Machete.Data;

namespace Machete.Service
{
    public interface IIdentityService
    {
        AuthenticationHeaderValue Authorization { get; set; }
        Task<HttpResponseMessage> PostAsync(string url, StringContent content);
        Task<HttpResponseMessage> GetAsync(string url);
    }
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        public IdentityService(HttpClient httpClient) => _httpClient = httpClient;
        AuthenticationHeaderValue IIdentityService.Authorization { get => _httpClient.DefaultRequestHeaders.Authorization; set => _httpClient.DefaultRequestHeaders.Authorization = value; }
        Task<HttpResponseMessage> IIdentityService.GetAsync(string url) => _httpClient.GetAsync(url);
        Task<HttpResponseMessage> IIdentityService.PostAsync(string url, StringContent content) => _httpClient.PostAsync(url, content);
    }
}
