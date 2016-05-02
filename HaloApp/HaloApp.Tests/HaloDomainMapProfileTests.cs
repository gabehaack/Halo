using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using HaloApp.Domain;
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

        [Fact]
        public void ValidConfiguration()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
