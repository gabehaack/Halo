using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using HaloApp.ApiClient;
using Xunit;
using System.Threading.Tasks;

namespace HaloApp.Tests
{
    [ExcludeFromCodeCoverage]
    public class ApiClientTests
    {
        private static readonly Uri HaloApiUri = new Uri(
            ConfigurationManager.AppSettings["HaloApiUri"]);
        private static readonly string SubscriptionKey =
            ConfigurationManager.AppSettings["SubscriptionKey"];

        [Fact]
        public async Task LegitTest()
        {
            var haloApi = new HaloApi(HaloApiUri, SubscriptionKey);
            await haloApi.GetMatchesAsync("shockRocket");
        }
    }
}
