using System;

namespace Simulator_Game
{
    /// <summary>
    /// Represents an active work session the player can start and stop.
    /// The system should call BeginEvent / EndEvent on GameTimeManager
    /// and award earnings when the session completes.
    /// </summary>
    public interface IWorkable
    {
        bool IsWorking { get; }

        void StartWorking();
        void StopWorking();

        /// <summary>Income earned from a completed work session.</summary>
        float GetEarnings();

        /// <summary>Fired when a work session finishes, passing the earnings amount.</summary>
        event Action<float> OnWorkCompleted;
    }
}
