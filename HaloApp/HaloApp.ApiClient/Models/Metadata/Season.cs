using System;
using System.Collections.Generic;

namespace HaloApp.ApiClient.Models.Metadata
{
    public class Season
    {
        public List<Playlist> playlists { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public Date startDate { get; set; }
        public Date endDate { get; set; }
        public bool isActive { get; set; }
        public Guid id { get; set; }
    }
}
