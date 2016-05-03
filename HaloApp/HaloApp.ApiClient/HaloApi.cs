using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using HaloApp.ApiClient.Models;
using HaloApp.ApiClient.Models.Metadata;
using HaloApp.Domain.Enums;
using HaloApp.Domain.Services;
using Newtonsoft.Json;
using DomainMetadata = HaloApp.Domain.Models.Metadata;
using DomainModels = HaloApp.Domain.Models.Dto;

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
            return csrDesignationMetadata
                .Select(Mapper.Map<DomainMetadata.CsrDesignation>);
        }

        public async Task<IEnumerable<DomainMetadata.FlexibleStat>> GetFlexibleStatMetadataAsync()
        {
            var flexibleStatMetadata = await GetMetadataAsync<FlexibleStat>("flexible-stats");
            return flexibleStatMetadata
                .Select(Mapper.Map<DomainMetadata.FlexibleStat>);
        }

        public async Task<IEnumerable<DomainMetadata.GameBaseVariant>> GetGameBaseVariantMetadataAsync()
        {
            var gameBaseVariantMetadata = await GetMetadataAsync<GameBaseVariant>("game-base-variants");
            return gameBaseVariantMetadata
                .Select(Mapper.Map<DomainMetadata.GameBaseVariant>);
        }

        public async Task<DomainMetadata.GameVariant> GetGameVariantMetadatumAsync(Guid gameVariantId)
        {
            var gameVariantMetadatum = await GetMetadatumAsync<GameVariant>("game-variants/", gameVariantId);
            return Mapper.Map<DomainMetadata.GameVariant>(gameVariantMetadatum);
        }

        public async Task<IEnumerable<DomainMetadata.Impulse>> GetImpulseMetadataAsync()
        {
            var impulseMetadata = await GetMetadataAsync<Impulse>("impulses");
            return impulseMetadata
                .Select(Mapper.Map<DomainMetadata.Impulse>);
        }

        public async Task<DomainMetadata.MapVariant> GetMapVariantMetadatumAsync(Guid mapVariantId)
        {
            var mapVariantMetadatum = await GetMetadatumAsync<MapVariant>("map-variants/", mapVariantId);
            return Mapper.Map<DomainMetadata.MapVariant>(mapVariantMetadatum);
        }

        public async Task<IEnumerable<DomainMetadata.Map>> GetMapMetadataAsync()
        {
            var mapMetadata = await GetMetadataAsync<Map>("maps");
            return mapMetadata
                .Select(Mapper.Map<DomainMetadata.Map>);
        }

        public async Task<IEnumerable<DomainMetadata.Medal>> GetMedalMetadataAsync()
        {
            var medalMetadata = await GetMetadataAsync<Medal>("medals");
            return medalMetadata
                .Select(Mapper.Map<DomainMetadata.Medal>);
        }

        public async Task<IEnumerable<DomainMetadata.Playlist>> GetPlaylistMetadataAsync()
        {
            var playlistMetadata = await GetMetadataAsync<Playlist>("playlists");
            return playlistMetadata
                .Select(Mapper.Map<DomainMetadata.Playlist>);
        }

        public async Task<IEnumerable<DomainMetadata.Season>> GetSeasonMetadataAsync()
        {
            var seasonMetadata = await GetMetadataAsync<Season>("seasons");
            return seasonMetadata
                .Select(Mapper.Map<DomainMetadata.Season>);
        }

        public async Task<IEnumerable<DomainMetadata.SpartanRank>> GetSpartanRankMetadataAsync()
        {
            var spartanRankMetadata = await GetMetadataAsync<SpartanRank>("spartan-ranks");
            return spartanRankMetadata
                .Select(Mapper.Map<DomainMetadata.SpartanRank>);
        }

        public async Task<IEnumerable<DomainMetadata.TeamColor>> GetTeamColorMetadataAsync()
        {
            var teamColorMetadata = await GetMetadataAsync<TeamColor>("team-colors");
            return teamColorMetadata
                .Select(Mapper.Map<DomainMetadata.TeamColor>);
        }

        public async Task<IEnumerable<DomainMetadata.Vehicle>> GetVehicleMetadataAsync()
        {
            var vehicleMetadata = await GetMetadataAsync<Vehicle>("vehicles");
            return vehicleMetadata
                .Select(Mapper.Map<DomainMetadata.Vehicle>);
        }

        public async Task<IEnumerable<DomainMetadata.Weapon>> GetWeaponMetadataAsync()
        {
            var weaponMetadata = await GetMetadataAsync<Weapon>("weapons");
            return weaponMetadata
                .Select(Mapper.Map<DomainMetadata.Weapon>);
        }

        #endregion

        #region Match Data

        public async Task<IEnumerable<DomainModels.MatchDto>> GetMatchesAsync(string player, int start = 0, int quantity = 25)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["modes"] = (GameMode.Arena).ToString().ToLower();
            queryString["start"] = start.ToString();
            queryString["count"] = quantity.ToString();
            string endpoint = String.Format("/players/{0}/matches?{1}", player, queryString);

            var playerMatches = await GetStatsAsync<PlayerMatches>(endpoint);
            return playerMatches.Results
                .Select(Mapper.Map<DomainModels.MatchDto>);
        }

        public async Task<IEnumerable<DomainModels.PlayerDto>> GetMatchStatsAsync(Guid matchId)
        {
            var matchReport = await GetMatchReportAsync(matchId);
            return matchReport.PlayerStats
                .Select(Mapper.Map<DomainModels.PlayerDto>);
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

        private async Task<IEnumerable<T>> GetMetadataAsync<T>(string entity)
        {
            var response = await GetAsync("/metadata/h5/metadata/" + entity);
            return await DeserializeContentAsync<List<T>>(response);
        }

        private async Task<T> GetMetadatumAsync<T>(string entity, Guid metadatumId)
        {
            var response = await GetAsync(String.Format("/metadata/h5/metadata/{0}/{1}", entity, metadatumId));
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
