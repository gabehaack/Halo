using System;
using System.Linq;
using System.Xml;
using AutoMapper;
using GHaack.Utilities;
using HaloApp.Domain.Enums;
using ApiMetadata = HaloApp.ApiClient.Models.Metadata;
using ApiModels = HaloApp.ApiClient.Models;
using DomainMetadata = HaloApp.Domain.Models.Metadata;
using DomainModels = HaloApp.Domain.Models.Dto;

namespace HaloApp.ApiClient
{
    public class HaloApiMapProfile : Profile
    {
        protected override void Configure()
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
                .ForMember(dest => dest.CsrDesignationId, opt => opt.MapFrom(
                   src => src.DesignationId))
                .ForMember(dest => dest.CsrDesignationTierId, opt => opt.MapFrom(
                   src => src.Tier))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(
                   src => src.Csr));

            CreateMap<ApiModels.MatchWeapon, DomainModels.WeaponStatsDto>()
                .ForMember(dest => dest.WeaponId, opt => opt.MapFrom(
                   src => src.WeaponId.StockId));

            CreateMap<ApiModels.MatchPlayerStats, DomainModels.PlayerDto>()
                .ForMember(dest => dest.AvgLifeTime, opt => opt.MapFrom(
                   src => src.AvgLifeTimeOfPlayer))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(
                   src => src.Player.Gamertag))
                .ForMember(dest => dest.WeaponsStats, opt => opt.MapFrom(
                   src => src.WeaponStats));

            CreateMap<ApiModels.MatchTeamStats, DomainModels.TeamDto>();

            CreateMap<ApiModels.PlayerMatch, DomainModels.MatchDto>()
                .ForMember(dest => dest.Completed, opt => opt.MapFrom(
                   src => src.MatchCompletedDate.ISO8601Date))
                .ForMember(dest => dest.GameMode, opt => opt.MapFrom(
                   src => src.Id.GameMode))
                .ForMember(dest => dest.GameVariantId, opt => opt.MapFrom(
                   src => src.GameVariant.ResourceId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(
                   src => src.Id.MatchId))
                .ForMember(dest => dest.MapVariantId, opt => opt.MapFrom(
                   src => src.MapVariant.ResourceId))
                .ForMember(dest => dest.PlaylistId, opt => opt.MapFrom(
                   src => src.HopperId))
                .ForMember(dest => dest.Players, opt => opt.Ignore())
                .ForMember(dest => dest.Teams, opt => opt.Ignore());

            CreateMap<ApiModels.MatchReport, DomainModels.MatchDto>()
                .ForAllMembers(opt => opt.Ignore());
            CreateMap<ApiModels.MatchReport, DomainModels.MatchDto>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(
                   src => src.PlayerStats))
                .ForMember(dest => dest.Teams, opt => opt.MapFrom(
                   src => src.TeamStats));
        }

        #region Type Conversions

        public class UriTypeConverter : ITypeConverter<string, Uri>
        {
            public Uri Convert(ResolutionContext context)
            {
                string source = (string) context.SourceValue;
                Uri uri;
                return Uri.TryCreate(source, UriKind.Absolute, out uri)
                    ? uri
                    : null;
            }
        }

        public class GuidTypeConverter : ITypeConverter<string, Guid>
        {
            public Guid Convert(ResolutionContext context)
            {
                string source = (string) context.SourceValue;
                Guid guid;
                return Guid.TryParse(source, out guid)
                    ? guid
                    : Guid.Empty;
            }
        }

        public class TimeSpanTypeConverter : ITypeConverter<string, TimeSpan>
        {
            public TimeSpan Convert(ResolutionContext context)
            {
                string source = (string) context.SourceValue;
                return !String.IsNullOrWhiteSpace(source)
                    ? XmlConvert.ToTimeSpan(source)
                    : TimeSpan.Zero;
            }
        }

        public class GameModeTypeConverter : ITypeConverter<string, GameMode>
        {
            public GameMode Convert(ResolutionContext context)
            {
                string source = (string) context.SourceValue;
                return EnumUtility.Parse<GameMode>(source);
            }
        }

        public class FlexibleStatTypeTypeConverter : ITypeConverter<string, FlexibleStatType>
        {
            public FlexibleStatType Convert(ResolutionContext context)
            {
                string source = (string) context.SourceValue;
                return EnumUtility.Parse<FlexibleStatType>(source);
            }
        }

        public class MedalClassificationTypeConverter : ITypeConverter<string, MedalClassification>
        {
            public MedalClassification Convert(ResolutionContext context)
            {
                string source = (string) context.SourceValue;
                return EnumUtility.Parse<MedalClassification>(source);
            }
        }

        public class WeaponTypeTypeConverter : ITypeConverter<string, WeaponType>
        {
            public WeaponType Convert(ResolutionContext context)
            {
                string source = (string) context.SourceValue;
                return EnumUtility.Parse<WeaponType>(source);
            }
        }

        #endregion
    }
}
