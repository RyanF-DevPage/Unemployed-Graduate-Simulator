using System.Collections.Generic;
using UnityEngine;

namespace Simulator_Game
{
    public class ToDoPanelUI : MonoBehaviour
    {
        [SerializeField] private WorkTodoEntryUI entryPrefab;
        [SerializeField] private Transform       container;

        private readonly Dictionary<JobData, WorkTodoEntryUI> _entries = new();

        private void Start()
        {
            ApplicationStateManager.Instance.OnApplicationStatusChanged += OnStatusChanged;
            EmailManager.Instance.OnMailboxChanged += SyncFromState; // catches Reset
            SyncFromState();
        }

        private void OnDestroy()
        {
            if (ApplicationStateManager.Instance != null)
                ApplicationStateManager.Instance.OnApplicationStatusChanged -= OnStatusChanged;
            if (EmailManager.Instance != null)
                EmailManager.Instance.OnMailboxChanged -= SyncFromState;
        }

        private void OnStatusChanged(JobData job, ApplicationStatus status)
        {
            if (status == ApplicationStatus.Accepted) AddEntry(job);
            else                                      RemoveEntry(job);
        }

        private void SyncFromState()
        {
            var accepted = ApplicationStateManager.Instance.GetJobsWithStatus(ApplicationStatus.Accepted);
            var acceptedSet = new HashSet<JobData>(accepted);

            // Remove entries for jobs no longer accepted
            var stale = new List<JobData>();
            foreach (var kvp in _entries)
                if (!acceptedSet.Contains(kvp.Key)) stale.Add(kvp.Key);
            foreach (var j in stale) RemoveEntry(j);

            // Add missing entries
            foreach (var j in accepted)
                if (!_entries.ContainsKey(j)) AddEntry(j);
        }

        private void AddEntry(JobData job)
        {
            if (_entries.ContainsKey(job)) return;
            var entry = Instantiate(entryPrefab, container);
            entry.Setup(job);
            _entries[job] = entry;
        }

        private void RemoveEntry(JobData job)
        {
            if (!_entries.TryGetValue(job, out var entry)) return;
            Destroy(entry.gameObject);
            _entries.Remove(job);
        }
    }
}
