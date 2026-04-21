namespace Simulator_Game
{
    /// <summary>
    /// Anything that modifies a PlayerStat — food items, entertainment activities,
    /// random events, or passive time-based depletion.
    /// </summary>
    public interface IStatAffector
    {
        /// <summary>Apply the effect (buff or debuff) via the stats manager.</summary>
        void ApplyEffect(PlayerStatsManager manager);

        /// <summary>Magnitude of the change — positive restores, negative depletes.</summary>
        float EffectAmount { get; }
    }
}
