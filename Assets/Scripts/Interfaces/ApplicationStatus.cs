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
        OfferReceived,  // Company offered — awaiting player response
        Accepted,       // Player confirmed the offer — going to work
        Rejected        // Rejected at any stage
    }
}
