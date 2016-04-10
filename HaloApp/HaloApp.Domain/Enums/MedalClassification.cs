using System.ComponentModel;

namespace HaloApp.Domain.Enums
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
