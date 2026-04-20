namespace Simulator_Game
{
    /// <summary>
    /// Implemented by systems that need to react to in-game time passing —
    /// e.g. passive stat depletion, job application wait timers, or scheduled events.
    /// Register with GameTimeManager.OnTimeChanged to receive ticks.
    /// </summary>
    public interface ITimeDependent
    {
        /// <summary>Called every in-game minute tick by GameTimeManager.</summary>
        void OnTimeUpdated(int day, int hour, int minute);
    }
}
