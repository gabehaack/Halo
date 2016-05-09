using System;
using GHaack.Halo.Domain.Models;

namespace GHaack.Halo.Web.Models
{
    public class PlayerViewModel
    {
        public string Player { get; set; }
        public Uri PlayerEmblemImageUri { get; set; }
        public PlayerStats Stats { get; set; }
    }
}