using AutoMapper;
using GHaack.Halo.Api;
using GHaack.Halo.Domain;

namespace GHaack.Halo.Web
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<HaloApiMapProfile>();
                cfg.AddProfile<HaloDomainMapProfile>();
            });
        }
    }
}