using System;

namespace Simulator_Game
{
    public interface IJobApplication
    {
        ApplicationStatus GetStatus(JobData job);
        bool TryApply(JobData job);
        bool CanAdvanceTo(JobData job, ApplicationStatus target);
        event Action<JobData, ApplicationStatus> OnApplicationStatusChanged;
    }
}
