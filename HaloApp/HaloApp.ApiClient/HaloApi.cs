using HaloApp.ApiClient.Models;
using HaloApp.Domain;
using HaloApp.Domain.Enums;
using HaloApp.Domain.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HaloApp.ApiClient
{
    public class HaloApi : IHaloApi
    {
        private readonly HttpClient _httpClient;

        public HaloApi(Uri baseUri, string subscriptionKey)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = baseUri,
            };
            _httpClient.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        #region Metadata

        public async Task<IList<Domain.Models.Map>> GetMapsAsync()
        {
            var response = await GetMetadataAsync("maps");
            var maps = await DeserializeContentAsync<List<Map>>(response);
            return maps.Select(m => m.ToDomainModel()).ToList();
        }

        public async Task<IList<Domain.Models.CsrDesignation>> GetCsrDesignationsAsync()
        {
            var response = await GetMetadataAsync("csr-designations");
            var maps = await DeserializeContentAsync<List<Map>>(response);
            return maps.Select(m => m.ToDomainModel()).ToList();
        }

        #endregion

        public async Task<IList<Domain.Models.Match>> GetMatchesAsync(string player, int start = 0, int count = 25)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["modes"] = ((int) GameMode.Arena).ToString();
            queryString["start"] = start.ToString();
            queryString["count"] = count.ToString();
            string endpoint = String.Format("/players/{0}/matches?{1}", player, queryString);
            var response = await GetStatsAsync(endpoint);
            var playerMatches = await DeserializeContentAsync<PlayerMatches>(response);
        }

        public async Task GetMatch(Guid matchId)
        {
            string endpoint = "/arena/matches/" + matchId;
            var response = await GetStatsAsync(endpoint);
            var matchReport = await DeserializeContentAsync<MatchReport>(response);
        }

        #region Generic Helpers

        private async Task<HttpResponseMessage> GetStatsAsync(string endpoint)
        {
            return await _httpClient.GetAsync("/stats/h5" + endpoint);
        }

        private async Task<HttpResponseMessage> GetMetadataAsync(string entity)
        {
            return await _httpClient.GetAsync("/metadata/h5/" + entity);
        }

        private async Task<T> DeserializeContentAsync<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        #endregion
    }
}
