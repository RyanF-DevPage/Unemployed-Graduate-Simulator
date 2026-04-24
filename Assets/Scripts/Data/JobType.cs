namespace Simulator_Game
{
    public enum JobType { FullTime, PartTime, Contract }

    public static class JobTypeExtensions
    {
        public static string ToDisplayString(this JobType t) => t switch
        {
            JobType.FullTime => "Full-time",
            JobType.PartTime => "Part-time",
            JobType.Contract => "Contract",
            _                => t.ToString()
        };
    }
}
