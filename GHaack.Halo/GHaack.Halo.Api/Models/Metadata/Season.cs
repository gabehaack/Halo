using System;
using System.Collections.Generic;

namespace GHaack.Halo.Api.Models.Metadata
{
    public class Season
    {
        public List<Playlist> playlists { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public bool isActive { get; set; }
        public string id { get; set; }
    }
}
