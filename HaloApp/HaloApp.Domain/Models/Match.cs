using HaloApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using HaloApp.Domain.Models.Metadata;

namespace HaloApp.Domain.Models
{
    public class Match
    {
        public DateTime Completed { get; set; }
        public GameMode GameMode { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid Id { get; set; }
        public Map Map { get; set; }
        public MapVariant MapVariant { get; set; }
        public GameBaseVariant GameBaseVariant { get; set; }
        public GameVariant GameVariant { get; set; }
        public Playlist Playlist { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public Season Season { get; set; }
        public bool TeamGame { get; set; }
        public IEnumerable<Team> Teams { get; set; }

        public Player GetPlayer(string player)
        {
            return Players.FirstOrDefault(p => p.Name == player);
        }
    }
}
