﻿using System;
using System.Collections.Generic;

namespace HaloApp.ApiClient.Models
{
    public class PlayerMatches
    {
        public int Start { get; set; }
        public int Count { get; set; }
        public int ResultCount { get; set; }
        public List<PlayerMatch> Results { get; set; }
    }

    public class PlayerMatch
    {
        public MatchIdClass Id { get; set; }
        public Guid HopperId { get; set; }
        public Guid MapId { get; set; }
        public MapVariant MapVariant { get; set; }
        public Guid GameBaseVariantId { get; set; }
        public GameVariant GameVariant { get; set; }
        public string MatchDuration { get; set; }
        public Date MatchCompletedDate { get; set; }
        public List<MatchTeam> Teams { get; set; }
        public bool IsTeamGame { get; set; }
        public Guid SeasonId { get; set; }
    }

    public class Date
    {
        public DateTime ISO8601Date { get; set; }
    }

    public class GameVariant
    {
        public int ResourceType { get; set; }
        public Guid ResourceId { get; set; }
        public int OwnerType { get; set; }
        public string Owner { get; set; }
    }

    public class MapVariant
    {
        public int ResourceType { get; set; }
        public Guid ResourceId { get; set; }
        public int OwnerType { get; set; }
        public string Owner { get; set; }
    }

    public class MatchIdClass
    {
        public Guid MatchId { get; set; }
        public int GameMode { get; set; }
    }

    public class MatchPlayer
    {
        public MatchPlayerId Player { get; set; }
        public int TeamId { get; set; }
        public int Rank { get; set; }
        public int Result { get; set; }
        public int TotalKills { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalAssists { get; set; }
    }

    public class MatchPlayerId
    {
        public string Gamertag { get; set; }
    }

    public class MatchTeam
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }
}
