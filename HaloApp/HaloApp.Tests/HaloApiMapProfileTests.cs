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
