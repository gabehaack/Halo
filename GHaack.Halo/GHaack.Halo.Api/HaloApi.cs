using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using GHaack.Halo.Api.Models;
using GHaack.Halo.Api.Models.Metadata;
using GHaack.Halo.Domain.Enums;
using GHaack.Halo.Domain.Services;
using Newtonsoft.Json;
using DomainMetadata = GHaack.Halo.Domain.Models.Metadata;
using DomainModels = GHaack.Halo.Domain.Models.Dto;

namespace GHaack.Halo.Api
{
    public class HaloApi : IHaloApi, IDisposable
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

        #region Player Profile Data

        public async Task<Uri> GetEmblemImageUriAsync(string player)
        {
            return await GetPlayerImageUriAsync(player, "emblem");
        }

        public async Task<Uri> GetSpartanImageUriAsync(string player)
        {
            return await GetPlayerImageUriAsync(player, "spartan");
        }

        #endregion

        #region Match Data

        public async Task<IEnumerable<DomainModels.MatchDto>> GetMatchesAsync(string player, int start = 0, int quantity = 25)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["modes"] = (GameMode.Arena).ToString().ToLower();
            queryString["start"] = start.ToString();
            queryString["count"] = quantity.ToString();
            string endpoint = $"/players/{player}/matches?{queryString}";

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

        #region Request Helpers

        private async Task<T> GetStatsAsync<T>(string endpoint)
        {
            var response = await GetAsync("/stats/h5" + endpoint);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default(T);
            response.EnsureSuccessStatusCode();
            return await DeserializeContentAsync<T>(response);
        }

        private async Task<IEnumerable<T>> GetMetadataAsync<T>(string entity)
        {
            var response = await GetAsync("/metadata/h5/metadata/" + entity);
            response.EnsureSuccessStatusCode();
            return await DeserializeContentAsync<List<T>>(response);
        }

        private async Task<T> GetMetadatumAsync<T>(string entity, Guid metadatumId)
        {
            var response = await GetAsync($"/metadata/h5/metadata/{entity}/{metadatumId}");
            response.EnsureSuccessStatusCode();
            return await DeserializeContentAsync<T>(response);
        }

        private async Task<Uri> GetPlayerImageUriAsync(string player, string imageName)
        {
            var response = await GetAsync($"/profile/h5/profiles/{player}/{imageName}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            response.EnsureSuccessStatusCode();
            return response.RequestMessage.RequestUri;
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
            return response;
        }

        private async Task<T> DeserializeContentAsync<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_httpClient != null)
                    {
                        _httpClient.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
