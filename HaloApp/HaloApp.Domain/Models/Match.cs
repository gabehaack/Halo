using HaloApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HaloApp.Domain.Models
{
    public class Match
    {
        public DateTime Completed { get; set; }
        public GameMode GameMode { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public Guid MapVariantId { get; set; }
        public Guid GameBaseVariantId { get; set; }
        public Guid GameVariantId { get; set; }
        public Guid PlaylistId { get; set; }
        public IList<MatchPlayer> Players { get; set; }
        public Guid SeasonId { get; set; }
        public bool TeamGame { get; set; }

        public MatchPlayer GetPlayer(string player)
        {
            return Players.FirstOrDefault(p => p.Name == player);
        }
    }
}
