using System;
using System.Collections.Generic;

namespace GHaack.Halo.Domain.Models.Metadata
{
    public class Season
    {
        public List<Guid> PlaylistIds { get; set; }
        public Uri IconUrl { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }
        public Guid Id { get; set; }
    }
}
