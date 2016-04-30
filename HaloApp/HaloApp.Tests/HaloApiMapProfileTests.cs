using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using HaloApp.ApiClient;
using Xunit;
using System.Threading.Tasks;
using MongoDB.Driver;
using HaloApp.Domain.Services;
using HaloApp.Data;
using AutoMapper;
using ApiModel = HaloApp.ApiClient.Models;
using ApiMetadata = HaloApp.ApiClient.Models.Metadata;
using DomainModel = HaloApp.Domain.Models;
using DomainMetadata = HaloApp.Domain.Models.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace HaloApp.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloApiMapProfileTests
    {
        public HaloApiMapProfileTests()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<HaloApiMapProfile>();
            });
        }

        [Fact]
        public void ValidConfiguration()
        {
            Mapper.AssertConfigurationIsValid();
        }

        #region Type Conversion

        [Fact]
        public void UriTypeConversion()
        {
            string source = "http://uri";
            var uri = Mapper.Map<Uri>(source);

            Assert.NotNull(uri);
            Assert.IsType<Uri>(uri);
            Assert.True(uri.Equals(new Uri(source)));
        }

        [Fact]
        public void UriTypeConversion_Null()
        {
            string source = null;
            var uri = Mapper.Map<Uri>(source);

            Assert.Null(uri);
        }

        [Fact]
        public void GuidTypeConversion()
        {
            var source = Guid.NewGuid();
            var guid = Mapper.Map<Guid>(source.ToString());

            Assert.NotNull(guid);
            Assert.IsType<Guid>(guid);
            Assert.Equal(source, guid);
        }

        [Fact]
        public void GuidTypeConversion_Null()
        {
            string source = null;
            var guid = Mapper.Map<Guid>(source);

            Assert.Equal(Guid.Empty, guid);
        }

        [Fact]
        public void TimeSpanTypeConversion()
        {
            string source = "PT1M30S";
            var timeSpan = Mapper.Map<TimeSpan>(source);

            Assert.NotNull(timeSpan);
            Assert.IsType<TimeSpan>(timeSpan);
            Assert.Equal(TimeSpan.FromSeconds(90), timeSpan);
        }

        [Fact]
        public void TimeSpanTypeConversion_Null()
        {
            string source = null;
            var timeSpan = Mapper.Map<TimeSpan>(source);

            Assert.Equal(TimeSpan.Zero, timeSpan);
        }
        
        #endregion

        [Fact]
        public void CsrTier()
        {
            var csrTier = Mapper.Map<DomainMetadata.CsrTier>(CsrTierData().First());

            Assert.Equal(new Uri("http://iconImageUrl"), csrTier.IconImageUrl);
            Assert.Equal(2, csrTier.Id);
        }

        [Fact]
        public void CsrDesignation()
        {
            var csrDesignation = Mapper.Map<DomainMetadata.CsrDesignation>(CsrDesignationData());

            Assert.Equal(new Uri("http://bannerImageUrl"), csrDesignation.BannerImageUrl);
            Assert.Equal(1, csrDesignation.Id);
            Assert.Equal("name", csrDesignation.Name);
            var csrTier = csrDesignation.Tiers.First();
            Assert.Equal(new Uri("http://iconImageUrl"), csrTier.IconImageUrl);
            Assert.Equal(2, csrTier.Id);
        }

        private static ApiMetadata.CsrDesignation CsrDesignationData()
        {
            return new ApiMetadata.CsrDesignation
            {
                bannerImageUrl = "http://bannerImageUrl",
                id = 1,
                name = "name",
                tiers = CsrTierData(),
            };
        }

        private static List<ApiMetadata.CsrTier> CsrTierData()
        {
            return new List<ApiMetadata.CsrTier>
            {
                new ApiMetadata.CsrTier
                {
                    iconImageUrl = "http://iconImageUrl",
                    id = 2,
                },
            };
        }
    }
}
