using AutoMapper;
using HaloApp.Domain.Models;
using HaloApp.Domain.Models.Dto;
using HaloApp.Domain.Models.Metadata;

namespace HaloApp.Domain
{
    public class HaloDomainMapProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<CsrDto, Csr>()
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(
                    src => new CsrDesignation { Id = src.CsrDesignationId }))
                .ForMember(dest => dest.Tier, opt => opt.MapFrom(
                    src => new CsrTier { Id = src.CsrDesignationTierId }));

            CreateMap<MatchDto, Match>()
                .ForMember(dest => dest.GameBaseVariant, opt => opt.MapFrom(
                    src => new GameBaseVariant { Id = src.GameBaseVariantId }))
                .ForMember(dest => dest.GameVariant, opt => opt.MapFrom(
                    src => new GameVariant { Id = src.GameVariantId }))
                .ForMember(dest => dest.Map, opt => opt.MapFrom(
                    src => new Map { Id = src.MapId }))
                .ForMember(dest => dest.MapVariant, opt => opt.MapFrom(
                    src => new MapVariant { Id = src.MapVariantId }))
                .ForMember(dest => dest.Playlist, opt => opt.MapFrom(
                    src => new Playlist { Id = src.PlaylistId }))
                .ForMember(dest => dest.Season, opt => opt.MapFrom(
                    src => new Season { Id = src.SeasonId }));

            CreateMap<PlayerDto, Player>();
            CreateMap<TeamDto, Team>();

            CreateMap<WeaponStatsDto, WeaponStats>()
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(
                    src => new Weapon { Id = src.WeaponId }));
        }
    }
}
