using System.Collections.Generic;
using UnityEngine;

namespace Simulator_Game
{
    public class JobBulletinPanelUI : MonoBehaviour
    {
        [SerializeField] private JobDatabase jobDatabase;
        [SerializeField] private PostingEntryUI postingPrefab;
        [SerializeField] private Transform jobsContainer;
        [SerializeField] private JobPostingPanelUI jobPostingPanel;

        private readonly List<PostingEntryUI> _entries = new();

        public void Refresh(int playerLevel)
        {
            foreach (var entry in _entries)
                Destroy(entry.gameObject);
            _entries.Clear();

            foreach (var job in jobDatabase.GetEligibleJobs(playerLevel))
            {
                var entry = Instantiate(postingPrefab, jobsContainer);
                entry.Setup(job, OnPostingClicked);
                _entries.Add(entry);
            }

            gameObject.SetActive(true);
        }

        private void OnPostingClicked(JobData job) => jobPostingPanel.Show(job);

        public void Hide() => gameObject.SetActive(false);
    }
}
