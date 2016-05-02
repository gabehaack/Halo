using System;
using System.Collections.Generic;
using HaloApp.Domain.Enums;

namespace HaloApp.Domain.Models.Dto
{
    public class MatchDto
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
        public IList<PlayerDto> Players { get; set; }
        public Guid SeasonId { get; set; }
        public bool TeamGame { get; set; }
        public IList<TeamDto> Teams { get; set; }
    }
}
