using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Simulator_Game
{
    public class ApplicationStateManager : MonoBehaviour, IJobApplication
    {
        [Header("Response Timer")]
        [SerializeField] private float responseWaitInGameMinutes = 60f;

        public static ApplicationStateManager Instance { get; private set; }

        public event Action<JobData, ApplicationStatus> OnApplicationStatusChanged;

        private readonly Dictionary<JobData, ApplicationStatus> _statuses = new();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<IJobApplication>(this);
        }

        private void OnDestroy()
        {
            if (Instance == this) ServiceLocator.Unregister<IJobApplication>();
        }

        // ── Queries ───────────────────────────────────────────────────────────

        public List<JobData> GetJobsWithStatus(ApplicationStatus status)
        {
            var result = new List<JobData>();
            foreach (var kvp in _statuses)
                if (kvp.Value == status) result.Add(kvp.Key);
            return result;
        }

        // ── Reset ─────────────────────────────────────────────────────────────

        public void Reset()
        {
            StopAllCoroutines();
            _statuses.Clear();
        }

        // ── IJobApplication ───────────────────────────────────────────────────

        public ApplicationStatus GetStatus(JobData job)
        {
            _statuses.TryGetValue(job, out var status);
            return status; // defaults to NotApplied
        }

        public bool TryApply(JobData job)
        {
            if (GetStatus(job) != ApplicationStatus.NotApplied) return false;
            SetStatus(job, ApplicationStatus.Pending);
            StartCoroutine(WaitAndRespond(job));
            return true;
        }

        private IEnumerator WaitAndRespond(JobData job)
        {
            float target = GameTimeManager.Instance.TotalMinutes + responseWaitInGameMinutes;
            yield return new WaitUntil(() => GameTimeManager.Instance.TotalMinutes >= target);
            if (GetStatus(job) != ApplicationStatus.Pending) yield break;
            if (!TryDirectOffer(job) && !TryAdvanceToInterview(job))
                SetStatus(job, ApplicationStatus.Rejected);
        }

        public bool CanAdvanceTo(JobData job, ApplicationStatus target)
        {
            var status = GetStatus(job);
            return target switch
            {
                ApplicationStatus.Pending   => status == ApplicationStatus.NotApplied,
                ApplicationStatus.Interview => status == ApplicationStatus.Pending,
                ApplicationStatus.Accepted  => status == ApplicationStatus.Pending
                                              || status == ApplicationStatus.Interview,
                ApplicationStatus.Rejected  => status == ApplicationStatus.Pending
                                              || status == ApplicationStatus.Interview,
                _                           => false,
            };
        }

        // ── The three game-driven transitions ─────────────────────────────────

        /// <summary>Pending → Interview. Game-driven (e.g. wait timer expired).</summary>
        public bool TryAdvanceToInterview(JobData job)
            => TryTransition(job, ApplicationStatus.Pending, ApplicationStatus.Interview,
                             job.interviewScreeningRate);

        /// <summary>Pending → Accepted. Game-driven direct offer (skips interview).</summary>
        public bool TryDirectOffer(JobData job)
            => TryTransition(job, ApplicationStatus.Pending, ApplicationStatus.Accepted,
                             job.directOfferRate);

        /// <summary>Interview → Accepted. Game-driven after interview evaluation.</summary>
        public bool TryPassInterview(JobData job)
            => TryTransition(job, ApplicationStatus.Interview, ApplicationStatus.Accepted,
                             job.interviewPassRate);

        // ── Internal ──────────────────────────────────────────────────────────

        private bool TryTransition(JobData job, ApplicationStatus required,
                                   ApplicationStatus target, float rate)
        {
            if (GetStatus(job) != required) return false;
            if (URandom.value > rate) return false;
            SetStatus(job, target);
            return true;
        }

        private void SetStatus(JobData job, ApplicationStatus status)
        {
            _statuses[job] = status;
            OnApplicationStatusChanged?.Invoke(job, status);
        }
    }
}
