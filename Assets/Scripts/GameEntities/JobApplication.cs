using System;

namespace Simulator_Game
{
    public class JobApplication : IJobApplication
    {
        public IJob Job { get; }
        public ApplicationStatus Status { get; private set; } = ApplicationStatus.NotApplied;
        public float WaitTimeMinutes => 0f;

        public event Action<ApplicationStatus> OnStatusChanged;

        public JobApplication(IJob job) { Job = job; }

        public void Apply()
        {
            if (Status != ApplicationStatus.NotApplied) return;
            Status = ApplicationStatus.Pending;
            OnStatusChanged?.Invoke(Status);
        }

        public void AdvanceStage()
        {
            // Future: Pending → Interview → Accepted/Rejected
        }
    }
}
