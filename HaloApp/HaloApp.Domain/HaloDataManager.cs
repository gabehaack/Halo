using HaloApp.Domain.Models;
using HaloApp.Domain.Models.Metadata;
using HaloApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloApp.Domain
{
    public class HaloDataManager
    {
        private readonly IHaloApi _haloApi;
        private readonly IHaloRepository _haloRepository;

        public HaloDataManager(IHaloApi haloApi, IHaloRepository haloRepository)
        {
            if (haloApi == null)
                throw new ArgumentNullException(nameof(haloApi));
            _haloApi = haloApi;

            if (haloRepository == null)
                throw new ArgumentNullException(nameof(haloRepository));
            _haloRepository = haloRepository;
        }

        #region Metadata

        public async Task ReplaceAllMetadataAsync()
        {
            Task csrDesignationTask = ReplaceCsrDesignationMetadataAsync();
            Task flexibleStatTask = ReplaceFlexibleStatMetadataAsync();
            Task gameBaseVariantTask = ReplaceGameBaseVariantMetadataAsync();
            //Task impulseTask = ReplaceImpulseMetadataAsync();
            Task mapTask = ReplaceMapMetadataAsync();
            Task medalTask = ReplaceMedalMetadataAsync();
            Task playlistTask = ReplacePlaylistMetadataAsync();
            Task seasonTask = ReplaceSeasonMetadataAsync();
            Task spartanRankTask = ReplaceSpartanRankMetadataAsync();
            Task teamColorTask = ReplaceTeamColorMetadataAsync();
            Task vehicleTask = ReplaceVehicleMetadataAsync();
            Task weaponTask = ReplaceWeaponMetadataAsync();

            await Task.WhenAll(new Task[] {
                csrDesignationTask,
                flexibleStatTask,
                gameBaseVariantTask,
                //impulseTask,
                mapTask,
                medalTask,
                playlistTask,
                seasonTask,
                spartanRankTask,
                teamColorTask,
                vehicleTask,
                weaponTask,
            });
        }

        public async Task ReplaceCsrDesignationMetadataAsync()
        {
            var csrDesignationMetadata = await _haloApi.GetCsrDesignationMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(csrDesignationMetadata.ToList());
        }

        public async Task ReplaceFlexibleStatMetadataAsync()
        {
            var flexibleStatMetadata = await _haloApi.GetFlexibleStatMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(flexibleStatMetadata.ToList());
        }

        public async Task ReplaceGameBaseVariantMetadataAsync()
        {
            var gameBaseVariantMetadata = await _haloApi.GetGameBaseVariantMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(gameBaseVariantMetadata.ToList());
        }

        //public async Task ReplaceImpulseMetadataAsync()
        //{
        //    var impulseMetadata = await _haloApi.GetImpulseMetadataAsync();
        //    await _haloRepository.ReplaceMetadata(impulseMetadata.ToList());
        //}

        public async Task ReplaceMapMetadataAsync()
        {
            var mapMetadata = await _haloApi.GetMapMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(mapMetadata.ToList());
        }

        public async Task ReplaceMedalMetadataAsync()
        {
            var medalMetadata = await _haloApi.GetMedalMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(medalMetadata.ToList());
        }

        public async Task ReplacePlaylistMetadataAsync()
        {
            var playlistMetadata = await _haloApi.GetPlaylistMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(playlistMetadata.ToList());
        }

        public async Task ReplaceSeasonMetadataAsync()
        {
            var seasonMetadata = await _haloApi.GetSeasonMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(seasonMetadata.ToList());
        }

        public async Task ReplaceSpartanRankMetadataAsync()
        {
            var spartanRankMetadata = await _haloApi.GetSpartanRankMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(spartanRankMetadata.ToList());
        }

        public async Task ReplaceTeamColorMetadataAsync()
        {
            var teamColorMetadata = await _haloApi.GetTeamColorMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(teamColorMetadata.ToList());
        }

        public async Task ReplaceVehicleMetadataAsync()
        {
            var vehicleMetadata = await _haloApi.GetVehicleMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(vehicleMetadata.ToList());
        }

        public async Task ReplaceWeaponMetadataAsync()
        {
            var weaponMetadata = await _haloApi.GetWeaponMetadataAsync();
            await _haloRepository.ReplaceMetadataAsync(weaponMetadata.ToList());
        }

        #endregion

        #region Match Data

        public async Task StoreMatchesAsync(string player)
        {
            var matches = await _haloApi.GetMatchesAsync(player);
            await _haloRepository.AddMatchesAsync(matches.ToList());
        }

        public async Task<IEnumerable<Match>> GetMatchesAsync(string player)
        {
            return await _haloRepository.GetMatchesAsync(player);
        }

        // TODO - medals and other raw data not being analyzed right now
        // Also, accuracy, k/d and some other things aren't showing up correctly,
        // accuracy is 0 so calculated wrong, k/d doesn't show 2nd decimal place

        public PlayerStats GetPlayerStats(IList<Match> matches, string player)
        {
            var matchPlayers = matches
                .Select(m => m.GetPlayer(player))
                .ToList();
            var weaponIds = matchPlayers
                .SelectMany(p => p.WeaponsStats.Select(w => w.Weapon.Id))
                .Distinct()
                .ToList();
            var weaponsStats = new List<WeaponStats>();
            foreach (var weaponId in weaponIds)
            {
                var weapons = matchPlayers
                    .Select(p => p.WeaponsStats.FirstOrDefault(w => w.Weapon.Id == weaponId))
                    .Where(w => w != null)
                    .ToList();
                weaponsStats.Add(new WeaponStats
                {
                    DamageDealt = weapons.Sum(w => w.DamageDealt),
                    Headshots = weapons.Sum(w => w.Headshots),
                    Weapon = weapons.First().Weapon,
                    Kills = weapons.Sum(w => w.Kills),
                    PossessionTime = TimeSpan.FromMilliseconds(weapons.Sum(w => w.PossessionTime.TotalMilliseconds)),
                    ShotsFired = weapons.Sum(w => w.ShotsFired),
                    ShotsLanded = weapons.Sum(w => w.ShotsLanded),
                });
            }
            return new PlayerStats
            {
                Assists = matchPlayers.Sum(p => p.Assists),
                DamageDealt = matchPlayers.Sum(p => p.DamageDealt),
                Deaths = matchPlayers.Sum(p => p.Deaths),
                GamesPlayed = matchPlayers.Count,
                Kills = matchPlayers.Sum(p => p.Kills),
                Name = player,
                ShotsFired = matchPlayers.Sum(p => p.ShotsFired),
                ShotsLanded = matchPlayers.Sum(p => p.ShotsLanded),
                TimePlayed = TimeSpan.FromMilliseconds(matches.Sum(m => m.Duration.TotalMilliseconds)),
                WeaponsStats = weaponsStats,
            };
        }

        #endregion

        public static double Round(double value)
        {
            return Math.Round(value, 1);
        }

        public static double RoundPercentage(double value)
        {
            return Math.Round(value, 3);
        }
    }
}
