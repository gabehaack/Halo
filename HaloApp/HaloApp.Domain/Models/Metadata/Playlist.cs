using HaloApp.Domain.Enums;
using System;

namespace HaloApp.Domain.Models.Metadata
{
    public class Playlist
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Ranked { get; set; }
        public Uri ImageUrl { get; set; }
        public GameMode GameMode { get; set; }
        public bool Active { get; set; }
        public Guid Id { get; set; }
    }
}
