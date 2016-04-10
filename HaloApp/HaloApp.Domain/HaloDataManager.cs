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
            await _haloRepository.ReplaceMetadata(csrDesignationMetadata.ToList());
        }

        public async Task ReplaceFlexibleStatMetadataAsync()
        {
            var flexibleStatMetadata = await _haloApi.GetFlexibleStatMetadataAsync();
            await _haloRepository.ReplaceMetadata(flexibleStatMetadata.ToList());
        }

        public async Task ReplaceGameBaseVariantMetadataAsync()
        {
            var gameBaseVariantMetadata = await _haloApi.GetGameBaseVariantMetadataAsync();
            await _haloRepository.ReplaceMetadata(gameBaseVariantMetadata.ToList());
        }

        //public async Task ReplaceImpulseMetadataAsync()
        //{
        //    var impulseMetadata = await _haloApi.GetImpulseMetadataAsync();
        //    await _haloRepository.ReplaceMetadata(impulseMetadata.ToList());
        //}

        public async Task ReplaceMapMetadataAsync()
        {
            var mapMetadata = await _haloApi.GetMapMetadataAsync();
            await _haloRepository.ReplaceMetadata(mapMetadata.ToList());
        }

        public async Task ReplaceMedalMetadataAsync()
        {
            var medalMetadata = await _haloApi.GetMedalMetadataAsync();
            await _haloRepository.ReplaceMetadata(medalMetadata.ToList());
        }

        public async Task ReplacePlaylistMetadataAsync()
        {
            var playlistMetadata = await _haloApi.GetPlaylistMetadataAsync();
            await _haloRepository.ReplaceMetadata(playlistMetadata.ToList());
        }

        public async Task ReplaceSeasonMetadataAsync()
        {
            var seasonMetadata = await _haloApi.GetSeasonMetadataAsync();
            await _haloRepository.ReplaceMetadata(seasonMetadata.ToList());
        }

        public async Task ReplaceSpartanRankMetadataAsync()
        {
            var spartanRankMetadata = await _haloApi.GetSpartanRankMetadataAsync();
            await _haloRepository.ReplaceMetadata(spartanRankMetadata.ToList());
        }

        public async Task ReplaceTeamColorMetadataAsync()
        {
            var teamColorMetadata = await _haloApi.GetTeamColorMetadataAsync();
            await _haloRepository.ReplaceMetadata(teamColorMetadata.ToList());
        }

        public async Task ReplaceVehicleMetadataAsync()
        {
            var vehicleMetadata = await _haloApi.GetVehicleMetadataAsync();
            await _haloRepository.ReplaceMetadata(vehicleMetadata.ToList());
        }

        public async Task ReplaceWeaponMetadataAsync()
        {
            var weaponMetadata = await _haloApi.GetWeaponMetadataAsync();
            await _haloRepository.ReplaceMetadata(weaponMetadata.ToList());
        }

        #endregion
    }
}
