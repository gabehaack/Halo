using System;

namespace HaloApp.Domain.Models.Metadata
{
    public class TeamColor
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public Uri IconUrl { get; set; }
        public int Id { get; set; }
    }
}
