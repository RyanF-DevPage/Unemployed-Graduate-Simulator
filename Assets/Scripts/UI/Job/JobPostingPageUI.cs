using System.Linq;
using TMPro;
using UnityEngine;

namespace Simulator_Game
{
    public class JobPostingPageUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subtitleText;
        [SerializeField] private TMP_Text bodyRequirements;
        [SerializeField] private TMP_Text bodyResponsibilities;
        [SerializeField] private TMP_Text bodyCompensation;

        public void Populate(JobData job)
        {
            titleText.text    = job.JobTitle;
            subtitleText.text = $"{job.CompanyName} · {job.JobType.ToDisplayString()}";
            bodyRequirements.text     = BuildBulletList(job.Requirements);
            bodyResponsibilities.text = BuildBulletList(job.Responsibilities);
            bodyCompensation.text     = BuildCompensation(job);
        }

        private static string BuildBulletList(string[] items)
        {
            if (items == null || items.Length == 0) return string.Empty;
            return string.Join("\n", items.Select(i => $"• {i}"));
        }

        private static string BuildCompensation(JobData job)
        {
            var benefits = BuildBulletList(job.Benefits);
            return benefits.Length > 0
                ? $"${job.BaseSalary:N0}/month\n\n{benefits}"
                : $"${job.BaseSalary:N0}/month";
        }
    }
}
