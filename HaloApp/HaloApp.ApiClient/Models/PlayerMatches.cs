using System;
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
        public string HopperId { get; set; }
        public string MapId { get; set; }
        public MapVariantId MapVariant { get; set; }
        public string GameBaseVariantId { get; set; }
        public GameVariantId GameVariant { get; set; }
        public string MatchDuration { get; set; }
        public Iso8601Date MatchCompletedDate { get; set; }
        public List<MatchTeam> Teams { get; set; }
        public bool IsTeamGame { get; set; }
        public string SeasonId { get; set; }
    }

    public class Iso8601Date
    {
        public DateTime ISO8601Date { get; set; }
    }

    public class GameVariantId
    {
        public string ResourceId { get; set; }
    }

    public class MapVariantId
    {
        public string ResourceId { get; set; }
    }

    public class MatchIdClass
    {
        public string MatchId { get; set; }
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
