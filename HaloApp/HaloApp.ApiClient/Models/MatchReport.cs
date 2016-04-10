using System;
using System.Collections.Generic;

namespace HaloApp.ApiClient.Models
{
    public class MatchReport
    {
        public List<MatchPlayerStats> PlayerStats { get; set; }
    }

    public class MatchPlayerStats
    {
        public MatchXpInfo XpInfo { get; set; }
        public MatchCsr PreviousCsr { get; set; }
        public MatchCsr CurrentCsr { get; set; }
        public int MeasurementMatchesLeft { get; set; }
        public List<KilledOpponentDetail> KilledOpponentDetails { get; set; }
        public List<KilledByOpponentDetail> KilledByOpponentDetails { get; set; }
        public MatchFlexibleStats FlexibleStats { get; set; }
        public CreditsEarned CreditsEarned { get; set; }
        public MatchPlayer Player { get; set; }
        public int TeamId { get; set; }
        public int Rank { get; set; }
        public bool DNF { get; set; }
        public string AvgLifeTimeOfPlayer { get; set; }
        public int TotalKills { get; set; }
        public int TotalHeadshots { get; set; }
        public double TotalWeaponDamage { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public MatchWeapon WeaponWithMostKills { get; set; }
        public int TotalMeleeKills { get; set; }
        public double TotalMeleeDamage { get; set; }
        public int TotalAssassinations { get; set; }
        public int TotalGroundPoundKills { get; set; }
        public double TotalGroundPoundDamage { get; set; }
        public int TotalShoulderBashKills { get; set; }
        public double TotalShoulderBashDamage { get; set; }
        public double TotalGrenadeDamage { get; set; }
        public int TotalPowerWeaponKills { get; set; }
        public double TotalPowerWeaponDamage { get; set; }
        public int TotalPowerWeaponGrabs { get; set; }
        public string TotalPowerWeaponPossessionTime { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalAssists { get; set; }
        public string TotalTimePlayed { get; set; }
        public int TotalGrenadeKills { get; set; }
        public List<MatchMedal> MedalAwards { get; set; }
        public List<VehiclesDestroyed> DestroyedEnemyVehicles { get; set; }
        public List<EnemyKills> EnemyKills { get; set; }
        public List<MatchWeapon> WeaponStats { get; set; }
        public List<MatchImpulse> Impulses { get; set; }
        public List<TeamStat> TeamStats { get; set; }
        public bool IsMatchOver { get; set; }
        public string TotalDuration { get; set; }
        public Guid MapVariantId { get; set; }
        public Guid GameVariantId { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid MapId { get; set; }
        public Guid GameBaseVariantId { get; set; }
        public bool IsTeamGame { get; set; }
        public Guid SeasonId { get; set; }
    }

    public class CreditsEarned
    {
        public int Result { get; set; }
        public int TotalCreditsEarned { get; set; }
        public double SpartanRankModifier { get; set; }
        public int PlayerRankAmount { get; set; }
        public double TimePlayedAmount { get; set; }
        public int BoostAmount { get; set; }
    }

    public class Enemy
    {
        public int BaseId { get; set; }
        public List<int> Attachments { get; set; }
    }

    public class EnemyKills
    {
        public Enemy Enemy { get; set; }
        public int TotalKills { get; set; }
    }

    public class MatchFlexibleStat
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
    }

    public class MatchFlexibleStats
    {
        public List<MatchFlexibleStat> MedalStatCounts { get; set; }
        public List<MatchFlexibleStat> ImpulseStatCounts { get; set; }
        public List<MatchFlexibleStat> MedalTimelapses { get; set; }
        public List<MatchFlexibleStat> ImpulseTimelapses { get; set; }
    }

    public class MatchImpulse
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }

    public class KilledByOpponentDetail
    {
        public string GamerTag { get; set; }
        public int TotalKills { get; set; }
    }

    public class KilledOpponentDetail
    {
        public string GamerTag { get; set; }
        public int TotalKills { get; set; }
    }

    public class MatchCsr
    {
        public int Tier { get; set; }
        public int DesignationId { get; set; }
        public int Csr { get; set; }
        public int PercentToNextTier { get; set; }
        public int Rank { get; set; }
    }

    public class MatchWeaponId
    {
        public int StockId { get; set; }
        public List<int> Attachments { get; set; }
    }

    public class MatchWeapon
    {
        public MatchWeaponId WeaponId { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public int TotalHeadshots { get; set; }
        public int TotalKills { get; set; }
        public double TotalDamageDealt { get; set; }
        public string TotalPossessionTime { get; set; }
    }

    public class MatchXpInfo
    {
        public int PrevSpartanRank { get; set; }
        public int SpartanRank { get; set; }
        public int PrevTotalXP { get; set; }
        public int TotalXP { get; set; }
        public double SpartanRankMatchXPScalar { get; set; }
        public int PlayerTimePerformanceXPAward { get; set; }
        public int PerformanceXP { get; set; }
        public int PlayerRankXPAward { get; set; }
        public int BoostAmount { get; set; }
    }

    public class MatchMedal
    {
        public int MedalId { get; set; }
        public int Count { get; set; }
    }

    public class RoundStat
    {
        public int RoundNumber { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
    }

    public class TeamStat
    {
        public int TeamId { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public List<RoundStat> RoundStats { get; set; }
    }

    public class VehiclesDestroyed
    {
        public Enemy Enemy { get; set; }
        public int TotalKills { get; set; }
    }
}