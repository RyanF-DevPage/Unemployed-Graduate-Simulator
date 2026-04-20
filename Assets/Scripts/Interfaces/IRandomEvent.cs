namespace Simulator_Game
{
    /// <summary>
    /// A random event that can fire during gameplay and affect player stats or state.
    /// The event manager should poll CanTrigger() periodically and call Trigger() when true.
    /// </summary>
    public interface IRandomEvent
    {
        string EventName { get; }
        string EventDescription { get; }

        /// <summary>0–1 probability of this event firing per evaluation window.</summary>
        float TriggerProbability { get; }

        /// <summary>Whether all conditions are met for this event to fire right now.</summary>
        bool CanTrigger();

        void Trigger();
    }
}
