namespace Simulator_Game
{
    /// <summary>
    /// Tracks where a job application currently stands in the pipeline.
    /// </summary>
    public enum ApplicationStatus
    {
        NotApplied,
        Pending,        // Applied — waiting for a response
        Interview,      // Passed initial screening — in behavioural interview stage
        Accepted,       // Job landed
        Rejected        // Rejected at any stage
    }
}
