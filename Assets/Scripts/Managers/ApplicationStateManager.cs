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
        private readonly Dictionary<JobData, int> _rejectedDay = new();

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
            _rejectedDay.Clear();
        }

        // ── IJobApplication ───────────────────────────────────────────────────

        public ApplicationStatus GetStatus(JobData job)
        {
            _statuses.TryGetValue(job, out var status);
            return status; // defaults to NotApplied
        }

        public bool TryApply(JobData job)
        {
            var status = GetStatus(job);
            if (status == ApplicationStatus.Rejected)
            {
                if (_rejectedDay.TryGetValue(job, out int day) && day >= GameTimeManager.Instance.CurrentDay)
                    return false;
                // New day — fall through and allow re-apply
            }
            else if (status != ApplicationStatus.NotApplied) return false;
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
            {
                _rejectedDay[job] = GameTimeManager.Instance.CurrentDay;
                SetStatus(job, ApplicationStatus.Rejected);
            }
        }

        public bool CanAdvanceTo(JobData job, ApplicationStatus target)
        {
            var status = GetStatus(job);
            return target switch
            {
                ApplicationStatus.Pending      => status == ApplicationStatus.NotApplied,
                ApplicationStatus.Interview    => status == ApplicationStatus.Pending,
                ApplicationStatus.OfferReceived => status == ApplicationStatus.Pending
                                               || status == ApplicationStatus.Interview,
                ApplicationStatus.Accepted     => status == ApplicationStatus.OfferReceived,
                ApplicationStatus.Rejected     => status == ApplicationStatus.Pending
                                               || status == ApplicationStatus.Interview
                                               || status == ApplicationStatus.OfferReceived,
                _                              => false,
            };
        }

        // ── Player offer response ─────────────────────────────────────────────

        /// <summary>
        /// Player accepts an offer. Returns false if work hours overlap an existing job.
        /// </summary>
        public bool AcceptOffer(JobData job)
        {
            if (GetStatus(job) != ApplicationStatus.OfferReceived) return false;
            if (HasWorkTimeConflict(job)) return false;
            SetStatus(job, ApplicationStatus.Accepted);
            return true;
        }

        /// <summary>
        /// Player declines an offer. Blocks re-application until the next in-game day.
        /// </summary>
        public void RejectOffer(JobData job)
        {
            if (GetStatus(job) != ApplicationStatus.OfferReceived) return;
            _rejectedDay[job] = GameTimeManager.Instance.CurrentDay;
            SetStatus(job, ApplicationStatus.Rejected);
        }

        private bool HasWorkTimeConflict(JobData newJob)
        {
            foreach (var job in GetJobsWithStatus(ApplicationStatus.Accepted))
                if (!(newJob.workStartHour >= job.workEndHour || newJob.workEndHour <= job.workStartHour))
                    return true;
            return false;
        }

        // ── Post-interview evaluation ─────────────────────────────────────────

        /// <summary>
        /// Schedules a post-interview decision after responseWaitInGameMinutes.
        /// scoreModifier (sum of selected answer modifiers, in [-0.9, 0.9]) is
        /// added to the job's interviewPassRate and clamped to [0, 1].
        /// </summary>
        public void PostInterview(JobData job, float scoreModifier)
        {
            StartCoroutine(WaitAndEvaluateInterview(job, scoreModifier));
        }

        private IEnumerator WaitAndEvaluateInterview(JobData job, float scoreModifier)
        {
            float target = GameTimeManager.Instance.TotalMinutes + responseWaitInGameMinutes;
            yield return new WaitUntil(() => GameTimeManager.Instance.TotalMinutes >= target);
            if (GetStatus(job) != ApplicationStatus.Interview) yield break;

            float effectiveRate = Mathf.Clamp01(job.interviewPassRate + scoreModifier);
            if (URandom.value <= effectiveRate)
                SetStatus(job, ApplicationStatus.OfferReceived);
            else
            {
                _rejectedDay[job] = GameTimeManager.Instance.CurrentDay;
                SetStatus(job, ApplicationStatus.Rejected);
            }
        }

        // ── The three game-driven transitions ─────────────────────────────────

        /// <summary>Pending → Interview. Game-driven (e.g. wait timer expired).</summary>
        public bool TryAdvanceToInterview(JobData job)
            => TryTransition(job, ApplicationStatus.Pending, ApplicationStatus.Interview,
                             job.interviewScreeningRate);

        /// <summary>Pending → OfferReceived. Game-driven direct offer (skips interview).</summary>
        public bool TryDirectOffer(JobData job)
            => TryTransition(job, ApplicationStatus.Pending, ApplicationStatus.OfferReceived,
                             job.directOfferRate);

        /// <summary>Interview → OfferReceived. Game-driven after interview evaluation.</summary>
        public bool TryPassInterview(JobData job)
            => TryTransition(job, ApplicationStatus.Interview, ApplicationStatus.OfferReceived,
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
