using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using HaloApp.Domain;
using HaloApp.Domain.Models;
using HaloApp.Domain.Models.Dto;
using Xunit;

namespace HaloApp.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloDomainMapProfileTests
    {
        public HaloDomainMapProfileTests()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<HaloDomainMapProfile>();
            });
        }

        #region Tests

        [Fact]
        public void ValidConfiguration()
        {
            Mapper.AssertConfigurationIsValid();
        }


        public void Csr()
        {
            var csr = Mapper.Map<Csr>(CsrDtoData()[0]);
            Assert.Equal(3, csr.PercentToNextTier);
            Assert.Null(csr.Rank);
            Assert.Equal(4, csr.Value);

            var csrDesignation = csr.Designation;
            Assert.NotNull(csrDesignation);
            Assert.Equal(1, csr.Designation.Id);

            var csrTier = csr.Tier;
            Assert.NotNull(csrTier);
            Assert.Equal(2, csr.Tier.Id);
        }
        #endregion

        #region Data

        private static Guid CsrDesignationGuid = Guid.NewGuid();
        private static IList<CsrDto> CsrDtoData()
        {
            return new List<CsrDto>
            {
                new CsrDto
                {
                    CsrDesignationId = 1,
                    CsrDesignationTierId = 2,
                    PercentToNextTier = 3,
                    Rank = null,
                    Value = 4,
                }
            };
        }

        #endregion
    }
}
