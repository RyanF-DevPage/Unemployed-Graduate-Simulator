namespace Simulator_Game
{
    /// <summary>
    /// Represents a job listing returned by the browser's job search.
    /// Holds all read-only data that describes a job opportunity.
    /// </summary>
    public interface IJob
    {
        string JobTitle { get; }
        string CompanyName { get; }
        float BaseSalary { get; }

        /// <summary>Minimum player level required to apply for this job.</summary>
        int RequiredLevel { get; }

        /// <summary>How many in-game minutes one work session takes.</summary>
        float WorkDurationMinutes { get; }

        /// <summary>Returns true if the player meets the level requirement to apply.</summary>
        bool IsEligible(int playerLevel);
    }
}
