using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloApp.Domain.Models
{
    public class Match
    {
        public string GameType { get; set; }
        public Guid Id { get; set; }
        public Map Map { get; set; }
        public Playlist Playlist { get; set; }
        public Season Season { get; set; }
        public List<MatchPlayer> Players { get; set; }
        public DateTime Completed { get; set; }
    }
}
