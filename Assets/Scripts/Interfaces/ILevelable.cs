using System;

namespace Simulator_Game
{
    /// <summary>
    /// Tracks the player's level and XP progression.
    /// Higher levels increase job eligibility and reduce application difficulty
    /// as described in the GDD rules & systems.
    /// </summary>
    public interface ILevelable
    {
        int   Level         { get; }
        float CurrentXP     { get; }
        float XPToNextLevel { get; }

        void AddXP(float amount);

        /// <summary>Fired with the new level when the player levels up.</summary>
        event Action<int> OnLevelUp;
    }
}
