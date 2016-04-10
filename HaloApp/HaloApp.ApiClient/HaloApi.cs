using HaloApp.ApiClient.Models;
using HaloApp.ApiClient.Models.Metadata;
using HaloApp.Domain.Enums;
using HaloApp.Domain.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using DomainMetadata = HaloApp.Domain.Models.Metadata;
using DomainModels = HaloApp.Domain.Models;

namespace HaloApp.ApiClient
{
    public class HaloApi : IHaloApi
    {
        private readonly HttpClient _httpClient;

        public HaloApi(Uri baseUri, string subscriptionKey)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = baseUri,
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        #region Metadata

        public async Task<IEnumerable<DomainMetadata.CsrDesignation>> GetCsrDesignationMetadataAsync()
        {
            var csrDesignationMetadata = await GetMetadataAsync<CsrDesignation>("csr-designations");
            return csrDesignationMetadata.Select(m => Mapper.CsrDesignation(m));
        }

        public async Task<IEnumerable<DomainMetadata.FlexibleStat>> GetFlexibleStatMetadataAsync()
        {
            var flexibleStatMetadata = await GetMetadataAsync<FlexibleStat>("flexible-stats");
            return flexibleStatMetadata.Select(m => Mapper.FlexibleStat(m));
        }

        public async Task<IEnumerable<DomainMetadata.GameBaseVariant>> GetGameBaseVariantMetadataAsync()
        {
            var gameBaseVariantMetadata = await GetMetadataAsync<GameBaseVariant>("game-base-variants");
            return gameBaseVariantMetadata.Select(m => Mapper.GameBaseVariant(m));
        }

        public async Task<DomainMetadata.GameVariant> GetGameVariantMetadatumAsync(Guid gameVariantId)
        {
            var gameVariantMetadatum = await GetMetadatumAsync<GameVariant>("game-variants/", gameVariantId);
            return Mapper.GameVariant(gameVariantMetadatum);
        }

        //public async Task<IEnumerable<DomainMetadata.Impulse>> GetImpulseMetadataAsync()
        //{
        //    var impulseMetadata = await GetMetadataAsync<Impulse>("impulses");
        //    return impulseMetadata.Select(m => Mapper.Impulse(m));
        //}

        public async Task<DomainMetadata.MapVariant> GetMapVariantMetadatumAsync(Guid mapVariantId)
        {
            var mapVariantMetadatum = await GetMetadatumAsync<MapVariant>("map-variants/", mapVariantId);
            return Mapper.MapVariant(mapVariantMetadatum);
        }

        public async Task<IEnumerable<DomainMetadata.Map>> GetMapMetadataAsync()
        {
            var mapMetadata = await GetMetadataAsync<Map>("maps");
            return mapMetadata.Select(m => Mapper.Map(m));
        }

        public async Task<IEnumerable<DomainMetadata.Medal>> GetMedalMetadataAsync()
        {
            var medalMetadata = await GetMetadataAsync<Medal>("medals");
            return medalMetadata.Select(m => Mapper.Medal(m));
        }

        public async Task<IEnumerable<DomainMetadata.Playlist>> GetPlaylistMetadataAsync()
        {
            var playlistMetadata = await GetMetadataAsync<Playlist>("playlists");
            return playlistMetadata.Select(m => Mapper.Playlist(m));
        }

        public async Task<IEnumerable<DomainMetadata.Season>> GetSeasonMetadataAsync()
        {
            var seasonMetadata = await GetMetadataAsync<Season>("seasons");
            return seasonMetadata.Select(m => Mapper.Season(m));
        }

        public async Task<IEnumerable<DomainMetadata.SpartanRank>> GetSpartanRankMetadataAsync()
        {
            var spartanRankMetadata = await GetMetadataAsync<SpartanRank>("spartan-ranks");
            return spartanRankMetadata.Select(m => Mapper.SpartanRank(m));
        }

        public async Task<IEnumerable<DomainMetadata.TeamColor>> GetTeamColorMetadataAsync()
        {
            var teamColorMetadata = await GetMetadataAsync<TeamColor>("team-colors");
            return teamColorMetadata.Select(m => Mapper.TeamColor(m));
        }

        public async Task<IEnumerable<DomainMetadata.Vehicle>> GetVehicleMetadataAsync()
        {
            var vehicleMetadata = await GetMetadataAsync<Vehicle>("vehicles");
            return vehicleMetadata.Select(m => Mapper.Vehicle(m));
        }

        public async Task<IEnumerable<DomainMetadata.Weapon>> GetWeaponMetadataAsync()
        {
            var weaponMetadata = await GetMetadataAsync<Weapon>("weapons");
            return weaponMetadata.Select(m => Mapper.Weapon(m));
        }

        #endregion

        #region Match Data

        public async Task<IEnumerable<DomainModels.Match>> GetMatchesAsync(string player, int start = 0, int count = 25)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["modes"] = ((int)GameMode.Arena).ToString();
            queryString["start"] = start.ToString();
            queryString["count"] = count.ToString();
            string endpoint = String.Format("/players/{0}/matches?{1}", player, queryString);
            var playerMatches = await GetStatsAsync<PlayerMatches>(endpoint);

            var matches = new HashSet<DomainModels.Match>();
            foreach (var playerMatch in playerMatches.Results)
            {
                var matchReport = await GetMatchReportAsync(playerMatch.Id.MatchId);
                matches.Add(Mapper.Match(playerMatch, matchReport));
            }
            return matches;
        }

        private async Task<MatchReport> GetMatchReportAsync(Guid matchId)
        {
            string endpoint = "/arena/matches/" + matchId;
            return await GetStatsAsync<MatchReport>(endpoint);
        }

        #endregion

        #region Generic Helpers

        private async Task<T> GetStatsAsync<T>(string endpoint)
        {
            var response = await GetAsync("/stats/h5" + endpoint);
            return await DeserializeContentAsync<T>(response);
        }

        private async Task<List<T>> GetMetadataAsync<T>(string entity)
        {
            var response = await GetAsync("/metadata/h5/metadata/" + entity);
            return await DeserializeContentAsync<List<T>>(response);
        }

        private async Task<T> GetMetadatumAsync<T>(string entity, Guid id)
        {
            var response = await GetAsync(String.Format("/metadata/h5/metadata/{0}/{1}", entity, id));
            return await DeserializeContentAsync<T>(response);
        }

        private async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            for (int i = 0; i < 10; i++)
            {
                if (response.IsSuccessStatusCode)
                    return response;
                if ((int) response.StatusCode == 429)
                {
                    await Task.Delay(1000);
                    response = await _httpClient.GetAsync(endpoint);
                }
            }
            response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<T> DeserializeContentAsync<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        #endregion
    }
}
