using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator_Game
{
    public class WorkManager : MonoBehaviour
    {
        public static WorkManager Instance { get; private set; }

        public event Action<JobData> OnWorkTimeReached;

        private bool _workInProgress;
        private readonly Dictionary<JobData, int> _acceptedDay = new();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            GameTimeManager.Instance.OnTimeChanged += OnTimeUpdated;
            ApplicationStateManager.Instance.OnApplicationStatusChanged += OnStatusChanged;
        }

        private void OnDestroy()
        {
            if (GameTimeManager.Instance != null)
                GameTimeManager.Instance.OnTimeChanged -= OnTimeUpdated;
            if (ApplicationStateManager.Instance != null)
                ApplicationStateManager.Instance.OnApplicationStatusChanged -= OnStatusChanged;
        }

        private void OnStatusChanged(JobData job, ApplicationStatus status)
        {
            if (status == ApplicationStatus.Accepted)
                _acceptedDay[job] = GameTimeManager.Instance.CurrentDay;
            else
                _acceptedDay.Remove(job);
        }

        private void OnTimeUpdated(int day, int hour, int minute)
        {
            if (_workInProgress || minute != 0) return;

            foreach (var job in ApplicationStateManager.Instance.GetJobsWithStatus(ApplicationStatus.Accepted))
            {
                if (hour == job.workStartHour
                    && _acceptedDay.TryGetValue(job, out int acceptedDay) && day > acceptedDay)
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
