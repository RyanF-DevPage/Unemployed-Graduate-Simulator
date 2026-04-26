using System;
using UnityEngine;

namespace Simulator_Game
{
    public class WorkManager : MonoBehaviour
    {
        public static WorkManager Instance { get; private set; }

        public event Action<JobData> OnWorkTimeReached;

        private bool _workInProgress;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            GameTimeManager.Instance.OnTimeChanged += OnTimeUpdated;
        }

        private void OnDestroy()
        {
            if (GameTimeManager.Instance != null)
                GameTimeManager.Instance.OnTimeChanged -= OnTimeUpdated;
        }

        private void OnTimeUpdated(int day, int hour, int minute)
        {
            if (_workInProgress || minute != 0) return;

            foreach (var job in ApplicationStateManager.Instance.GetJobsWithStatus(ApplicationStatus.Accepted))
            {
                if (hour == job.workStartHour)
                {
                    _workInProgress = true;
                    GameTimeManager.Instance.Pause();
                    OnWorkTimeReached?.Invoke(job);
                    return;
                }
            }
        }

        public void CompleteWork(JobData job)
        {
            int minutesToSkip = Mathf.RoundToInt(job.workDurationHours * 60);
            GameTimeManager.Instance.SkipMinutes(minutesToSkip);

            PlayerStatsManager.Instance.AddFunds(job.baseSalary * job.workDurationHours);
            PlayerStatsManager.Instance.ModifyHunger(
                -(PlayerStatsManager.Instance.HungerDepletionRate * job.workDurationHours * 5f));

            _workInProgress = false;
            GameTimeManager.Instance.Resume();
        }
    }
}
