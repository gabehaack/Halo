using System;
using System.Linq;
using System.Xml;
using AutoMapper;
using GHaack.Utilities;
using GHaack.Halo.Domain.Enums;
using ApiMetadata = GHaack.Halo.Api.Models.Metadata;
using ApiModels = GHaack.Halo.Api.Models;
using DomainMetadata = GHaack.Halo.Domain.Models.Metadata;
using DomainModels = GHaack.Halo.Domain.Models.Dto;

namespace GHaack.Halo.Api
{
    public class HaloApiMapProfile : Profile
    {
        public HaloApiMapProfile()
        {
            RecognizePrefixes("Total", "Is", "is", "Match");
            RecognizePostfixes("Id");

            // Type Conversions
            CreateMap<string, Uri>()
                .ConvertUsing<UriTypeConverter>();
            CreateMap<string, Guid>()
                .ConvertUsing<GuidTypeConverter>();
            CreateMap<string, TimeSpan>()
                .ConvertUsing<TimeSpanTypeConverter>();
            CreateMap<string, FlexibleStatType>()
                .ConvertUsing<FlexibleStatTypeTypeConverter>();
            CreateMap<string, GameMode>()
                .ConvertUsing<GameModeTypeConverter>();
            CreateMap<string, MedalClassification>()
                .ConvertUsing<MedalClassificationTypeConverter>();
            CreateMap<string, WeaponType>()
                .ConvertUsing<WeaponTypeTypeConverter>();

            // Metadata
            CreateMap<ApiMetadata.CsrDesignation, DomainMetadata.CsrDesignation>();
            CreateMap<ApiMetadata.CsrTier, DomainMetadata.CsrTier>();
            CreateMap<ApiMetadata.FlexibleStat, DomainMetadata.FlexibleStat>();
            CreateMap<ApiMetadata.GameBaseVariant, DomainMetadata.GameBaseVariant>();
            CreateMap<ApiMetadata.GameVariant, DomainMetadata.GameVariant>();
            CreateMap<ApiMetadata.Impulse, DomainMetadata.Impulse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(
                   src => src.internalName));
            CreateMap<ApiMetadata.Map, DomainMetadata.Map>();
            CreateMap<ApiMetadata.MapVariant, DomainMetadata.MapVariant>();
            CreateMap<ApiMetadata.Medal, DomainMetadata.Medal>();
            CreateMap<ApiMetadata.MedalSpriteLocation, DomainMetadata.MedalSpriteLocation>();
            CreateMap<ApiMetadata.Playlist, DomainMetadata.Playlist>();
            CreateMap<ApiMetadata.Season, DomainMetadata.Season>()
                .ForMember(dest => dest.PlaylistIds, opt => opt.MapFrom(
                   src => src.playlists.Select(p => p.id)));
            CreateMap<ApiMetadata.SpartanRank, DomainMetadata.SpartanRank>();
            CreateMap<ApiMetadata.TeamColor, DomainMetadata.TeamColor>();
            CreateMap<ApiMetadata.Vehicle, DomainMetadata.Vehicle>();
            CreateMap<ApiMetadata.Weapon, DomainMetadata.Weapon>();

            // Match stats
            CreateMap<ApiModels.MatchCsr, DomainModels.CsrDto>()
                .ForMember(dest => dest.CsrDesignationId,
                    opt => opt.MapFrom(src => src.DesignationId))
                .ForMember(dest => dest.CsrDesignationTierId,
                    opt => opt.MapFrom(src => src.Tier))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Csr));

            CreateMap<ApiModels.MatchWeapon, DomainModels.WeaponStatsDto>()
                .ForMember(dest => dest.WeaponId, opt => opt.MapFrom(
                   src => src.WeaponId.StockId));

            CreateMap<ApiModels.MatchPlayerStats, DomainModels.PlayerDto>()
                .ForMember(dest => dest.AvgLifeTime,
                    opt => opt.MapFrom(src => src.AvgLifeTimeOfPlayer))
                .ForMember(dest => dest.Name, opt =>
                    opt.MapFrom(src => src.Player.Gamertag))
                .ForMember(dest => dest.WeaponsStats,
                    opt => opt.MapFrom(src => src.WeaponStats));

            CreateMap<ApiModels.MatchTeamStats, DomainModels.TeamDto>();

            CreateMap<ApiModels.PlayerMatch, DomainModels.MatchDto>()
                .ForMember(dest => dest.Completed,
                    opt => opt.MapFrom(src => src.MatchCompletedDate.ISO8601Date))
                .ForMember(dest => dest.GameMode,
                    opt => opt.MapFrom(src => src.Id.GameMode))
                .ForMember(dest => dest.GameVariantId,
                    opt => opt.MapFrom(src => src.GameVariant.ResourceId))
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id.MatchId))
                .ForMember(dest => dest.MapVariantId,
                    opt => opt.MapFrom(src => src.MapVariant.ResourceId))
                .ForMember(dest => dest.PlaylistId,
                    opt => opt.MapFrom(src => src.HopperId))
                .ForMember(dest => dest.Players,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Teams,
                    opt => opt.Ignore());

            CreateMap<ApiModels.MatchReport, DomainModels.MatchDto>()
                .ForMember(dest => dest.Players,
                    opt => opt.MapFrom(src => src.PlayerStats))
                .ForMember(dest => dest.Teams,
                    opt => opt.MapFrom(src => src.TeamStats))
                .ForMember(dest => dest.Completed,
                    opt => opt.Ignore())
                .ForMember(dest => dest.GameMode,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Duration,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.MapId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.MapVariantId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.GameBaseVariantId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.GameVariantId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PlaylistId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SeasonId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.TeamGame,
                    opt => opt.Ignore());
        }

        #region Type Conversions

        public class UriTypeConverter : ITypeConverter<string, Uri>
        {
            public Uri Convert(string source, Uri destination, ResolutionContext context)
            {
                Uri uri;
                return Uri.TryCreate(source, UriKind.Absolute, out uri)
                    ? uri
                    : null;
            }
        }

        public class GuidTypeConverter : ITypeConverter<string, Guid>
        {
            public Guid Convert(string source, Guid destination, ResolutionContext context)
            {
                Guid guid;
                return Guid.TryParse(source, out guid)
                    ? guid
                    : Guid.Empty;
            }
        }

        public class TimeSpanTypeConverter : ITypeConverter<string, TimeSpan>
        {
            public TimeSpan Convert(string source, TimeSpan destination, ResolutionContext context)
            {
                return !String.IsNullOrWhiteSpace(source)
                    ? XmlConvert.ToTimeSpan(source)
                    : TimeSpan.Zero;
            }
        }

        public class GameModeTypeConverter : ITypeConverter<string, GameMode>
        {
            public GameMode Convert(string source, GameMode destination, ResolutionContext context)
            {
                return EnumUtility.Parse<GameMode>(source);
            }
        }

        public class FlexibleStatTypeTypeConverter : ITypeConverter<string, FlexibleStatType>
        {
            public FlexibleStatType Convert(string source, FlexibleStatType destination, ResolutionContext context)
            {
                return EnumUtility.Parse<FlexibleStatType>(source);
            }
        }

        public class MedalClassificationTypeConverter : ITypeConverter<string, MedalClassification>
        {
            public MedalClassification Convert(string source, MedalClassification destination, ResolutionContext context)
            {
                return EnumUtility.Parse<MedalClassification>(source);
            }
        }

        public class WeaponTypeTypeConverter : ITypeConverter<string, WeaponType>
        {
            public WeaponType Convert(string source, WeaponType destination, ResolutionContext context)
            {
                return EnumUtility.Parse<WeaponType>(source);
            }
        }

        #endregion
    }
}
