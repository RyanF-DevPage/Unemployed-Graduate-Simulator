using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    [RequireComponent(typeof(Button))]
    public class PostingEntryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text typeText;
        [SerializeField] private TMP_Text salaryText;

        private Button _button;

        private void Awake() => _button = GetComponent<Button>();

        public void Setup(JobData job, Action<JobData> onClick)
        {
            titleText.text    = job.JobTitle;
            typeText.text     = job.JobType.ToDisplayString();
            salaryText.text   = $"${job.BaseSalary:N0}/hour";

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onClick(job));
        }
    }
}
