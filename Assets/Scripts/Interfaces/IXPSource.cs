namespace Simulator_Game
{
    /// <summary>
    /// Any activity that rewards the player with experience points —
    /// eating food, entertainment, completing work sessions, etc.
    /// </summary>
    public interface IXPSource
    {
        /// <summary>Amount of XP granted when this activity is completed.</summary>
        int XPReward { get; }
    }
}
