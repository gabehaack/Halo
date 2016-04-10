using HaloApp.ApiClient.Models;
using HaloApp.ApiClient.Models.Metadata;
using HaloApp.Domain.Enums;
using HaloApp.Domain.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            _httpClient = new HttpClient
            {
                BaseAddress = baseUri,
            };
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

        public async Task<DomainMetadata.GameVariant> GetGameVariantAsync(Guid gameVariantId)
        {
            var gameVariantMetadata = await GetMetadataAsync<GameVariant>("game-variants/" + gameVariantId);
            return gameVariantMetadata.Select(m => Mapper.GameVariant(m));
        }

        public async Task<IEnumerable<DomainMetadata.Impulse>> GetImpulseMetadataAsync()
        {
            var impulseMetadata = await GetMetadataAsync<Impulse>("impulses");
            return impulseMetadata.Select(m => Mapper.Impulse(m));
        }

        public async Task<DomainMetadata.MapVariant> GetMapVariantMetadataAsync(Guid mapVariantId)
        {
            var mapVariantMetadata = await GetMetadataAsync<MapVariant>("map-variants/" + mapVariantId);
            return mapVariantMetadata.Select(m => Mapper.MapVariant(m));
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
            var seasonMetadata = await GetMetadataAsync<Season>("season");
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
            queryString["modes"] = ((int) GameMode.Arena).ToString();
            queryString["start"] = start.ToString();
            queryString["count"] = count.ToString();
            string endpoint = String.Format("/players/{0}/matches?{1}", player, queryString);
            var response = await GetStatsAsync(endpoint);
            var playerMatches = await DeserializeContentAsync<PlayerMatches>(response);

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
            var response = await GetStatsAsync(endpoint);
            return await DeserializeContentAsync<MatchReport>(response);
        }

        #endregion

        #region Generic Helpers

        private async Task<HttpResponseMessage> GetStatsAsync(string endpoint)
        {
            return await _httpClient.GetAsync("/stats/h5" + endpoint);
        }

        private async Task<List<T>> GetMetadataAsync<T>(string entity)
        {
            var response = await _httpClient.GetAsync("/metadata/h5/metadata/" + entity);
            return await DeserializeContentAsync<List<T>>(response);
        }

        private async Task<T> DeserializeContentAsync<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        #endregion
    }
}
