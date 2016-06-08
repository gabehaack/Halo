using AutoMapper;

namespace GHaack.Halo.Api
{
    public static class MapExtensions
    {
        public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source, IMapper mapper)
        {
            return mapper.Map(source, destination);
        }
    }
}
