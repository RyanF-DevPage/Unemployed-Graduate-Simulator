namespace Simulator_Game
{
    public interface IJob
    {
        string JobTitle { get; }
        string CompanyName { get; }
        JobType JobType { get; }
        float BaseSalary { get; }
        int RequiredLevel { get; }
        float WorkDurationMinutes { get; }
        string[] Requirements { get; }
        string[] Responsibilities { get; }
        string[] Benefits { get; }
        bool IsEligible(int playerLevel);
    }
}
