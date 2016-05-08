using System.ComponentModel;

namespace GHaack.Halo.Domain.Enums
{
    public enum MedalClassification
    {
        Unknown = 0,
        [Description("Multi-kill")]
        MultiKill,
        Spree,
        Style,
        Vehicle,
        Breakout,
        Objective
    }
}
