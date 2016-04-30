﻿using AutoMapper;

namespace HaloApp.ApiClient
{
    public static class MapExtensions
    {
        public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source)
        {
            return Mapper.Map(source, destination);
        }
    }
}
