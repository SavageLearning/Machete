using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Web.Maps.Api;
using Machete.Web.ViewModel.Api;
using Newtonsoft.Json;
// using SimpleJson;
using LookupViewModel = Machete.Web.ViewModel.Api.LookupVM;
using WorkAssignmentViewModel = Machete.Web.ViewModel.Api.WorkAssignmentVM;

namespace Machete.Test.Integration.HttpClientUtil
{
    /// <summary>
    /// The httpClient utility to grab Machete Lookups from the desired URL.
    /// Replaces static lookups and allows UI testing on deployed test environments
    /// </summary>
    public static class HttpClientUtil
    {
        private static readonly HttpClient HttpClient;
        private static readonly IMapper _mapper;

        public static List<Lookup> TenantLookupsCache => _tenantLookupsCache;
        private static List<Lookup> _tenantLookupsCache;
        
        /// <summary>
        /// Initializes httpClient instance with a cookie container
        /// Initializes automapper with the necessary profiles
        /// </summary>
        static HttpClientUtil()
        {
            HttpClientHandler clientHandler = 
                new HttpClientHandler();
            clientHandler.UseCookies = true;
            clientHandler.CookieContainer = new CookieContainer();
            HttpClient = new HttpClient(clientHandler);
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LookupsMap>();
                cfg.AddProfile<WorkAssignmentsMap>();
            }
            );
            _mapper = new Mapper(configuration);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">The base url for the Machete tenant e.g. https://tenant.example.com/</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task SetTenantLookUpCache(string url)
        {
            if (TenantLookupsCache is null)
            {
                var httpResponse = await HttpClient.GetAsync($"{url}api/lookups");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot retrieve records");
                }
                var content = await httpResponse.Content.ReadAsStringAsync();
                var deserializedContent = JsonConvert.DeserializeObject<ListResponseModel<LookupVM>>(content);
                var resultList = _mapper.Map<List<Lookup>>(deserializedContent.data);
                _tenantLookupsCache = new List<Lookup>(resultList);
            }
        }

        public static int GetLookup(string category, string text_EN)
        {
            var lu = _tenantLookupsCache.FirstOrDefault(x => x.category == category && x.text_EN == text_EN);
            if (lu is null)
            {
                throw new Exception("No record found");
            }
            return lu.ID;
        }
        
        public static int GetFirstLookupInCategory(string category)
        {
            var lu = _tenantLookupsCache.FirstOrDefault(x => x.category == category);
            if (lu is null)
            {
                throw new Exception("No record found");
            }
            return lu.ID;
        }
        
        public static string GetFirstLookupTextEn(int id)
        {
            var lu = _tenantLookupsCache.FirstOrDefault(x => x.ID == id);
            if (lu is null)
            {
                throw new Exception("No record found");
            }
            return lu.text_EN;
        }

        public static string TextEN(this int id) => GetFirstLookupTextEn(id);

        public static async Task<int> GetWorkAssignment(int id)
        {
            var waPseudoId = "";
            var creds = JsonConvert.SerializeObject(new
            {
                username = SharedConfig.SeleniumUser,
                passWord = SharedConfig.SeleniumUserPassword
            });
            var body = new StringContent(creds, Encoding.UTF8, "application/json");
            var httpResponse = await HttpClient.PostAsync($"{SharedConfig.BaseSeleniumTestUrl}id/login", body);
            httpResponse.EnsureSuccessStatusCode();
            var waResponse = await
                HttpClient.GetAsync($"{SharedConfig.BaseSeleniumTestUrl}api/workassignments/{id}");
            var httpResponseString = await waResponse.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<ItemResponseModel<WorkAssignmentVM>>(httpResponseString);
            var domainWA = _mapper.Map<WorkAssignment>(deserializedResponse.data);
            return domainWA.pseudoID ?? 0;
        }
    }
}
