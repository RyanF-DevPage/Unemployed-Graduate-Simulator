using System;

namespace Simulator_Game
{
    /// <summary>
    /// Tracks the lifecycle of a single job application:
    /// Apply → Pending → Interview → Accepted / Rejected.
    /// </summary>
    public interface IJobApplication
    {
        IJob Job { get; }
        ApplicationStatus Status { get; }

        /// <summary>In-game minutes until the next status update arrives.</summary>
        float WaitTimeMinutes { get; }

        void Apply();

        /// <summary>Move the application to the next stage (e.g. after an interview).</summary>
        void AdvanceStage();

        /// <summary>Fired whenever Status changes.</summary>
        event Action<ApplicationStatus> OnStatusChanged;
    }

}
