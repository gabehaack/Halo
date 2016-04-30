using GHaack.Utilities;
using System;
using System.Linq;
using System.Xml;
using AutoMapper;
using HaloApp.Domain.Enums;
using ApiModels = HaloApp.ApiClient.Models;
using ApiMetadata = HaloApp.ApiClient.Models.Metadata;
using DomainMetadata = HaloApp.Domain.Models.Metadata;
using DomainModels = HaloApp.Domain.Models;

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
            //CreateMap<Impulse, DomainMetadata.Impulse>();
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
            CreateMap<ApiModels.PlayerMatch, DomainModels.Match>()
                .ForMember(dest => dest.Completed, opt => opt.MapFrom(
                   src => src.MatchCompletedDate.ISO8601Date))
                .ForMember(dest => dest.GameMode, opt => opt.MapFrom(
                   src => src.Id.GameMode))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(
                   src => src.Id.MatchId))
                .ForMember(dest => dest.MapVariant, opt => opt.MapFrom(
                   src => src.MapVariant.ResourceId))
                .ForMember(dest => dest.GameVariantId, opt => opt.MapFrom(
                   src => src.GameVariant.ResourceId))
                .ForMember(dest => dest.PlaylistId, opt => opt.Ignore())
                .ForMember(dest => dest.Players, opt => opt.Ignore());
            CreateMap<ApiModels.MatchPlayerStats, DomainModels.Match>()
                .ForMember(dest => dest.Completed, opt => opt.Ignore())
                .ForMember(dest => dest.GameMode, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Players, opt => opt.Ignore());
            CreateMap<ApiModels.MatchReport, DomainModels.Match>()
                .ForAllMembers(opt => opt.Ignore());
            CreateMap<ApiModels.MatchReport, DomainModels.Match>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(
                   src => src.PlayerStats));
            CreateMap<ApiModels.MatchCsr, DomainModels.Csr>()
                .ForMember(dest => dest.CsrDesignationId,  opt => opt.MapFrom(
                    src => src.DesignationId))
                .ForMember(dest => dest.CsrDesignationTierId, opt => opt.MapFrom(
                   src => src.Tier))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(
                   src => src.Csr));
            CreateMap<ApiModels.MatchPlayerStats, DomainModels.MatchPlayer>()
                .ForMember(dest => dest.AvgLifeTime, opt => opt.MapFrom(
                   src => src.AvgLifeTimeOfPlayer))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(
                   src => src.Player.Gamertag))
                .ForMember(dest => dest.WeaponsStats, opt => opt.MapFrom(
                   src => src.WeaponStats));
            CreateMap<ApiModels.MatchWeapon, DomainModels.WeaponStats>()
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(
                   src => new DomainMetadata.Weapon { Id = src.WeaponId.StockId }));
        }

        #region Type Conversions

        public class UriTypeConverter : ITypeConverter<string, Uri>
        {
            public Uri Convert(ResolutionContext context)
            {
                string source = (string)context.SourceValue;
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
                string source = (string)context.SourceValue;
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
                string source = (string)context.SourceValue;
                return XmlConvert.ToTimeSpan(source);
            }
        }

        public class GameModeTypeConverter : ITypeConverter<string, GameMode>
        {
            public GameMode Convert(ResolutionContext context)
            {
                string source = (string)context.SourceValue;
                return EnumUtility.Parse<GameMode>(source);
            }
        }

        public class FlexibleStatTypeTypeConverter : ITypeConverter<string, FlexibleStatType>
        {
            public FlexibleStatType Convert(ResolutionContext context)
            {
                string source = (string)context.SourceValue;
                return EnumUtility.Parse<FlexibleStatType>(source);
            }
        }

        public class MedalClassificationTypeConverter : ITypeConverter<string, MedalClassification>
        {
            public MedalClassification Convert(ResolutionContext context)
            {
                string source = (string)context.SourceValue;
                return EnumUtility.Parse<MedalClassification>(source);
            }
        }

        public class WeaponTypeTypeConverter : ITypeConverter<string, WeaponType>
        {
            public WeaponType Convert(ResolutionContext context)
            {
                string source = (string)context.SourceValue;
                return EnumUtility.Parse<WeaponType>(source);
            }
        }

        #endregion
    }
}
