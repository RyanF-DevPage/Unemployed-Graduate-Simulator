using System.Text;
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
            subtitleText.text = $"{job.CompanyName} · {job.City} · {FormatJobType(job.JobType)}";
            bodyRequirements.text     = BuildBulletList(job.Requirements);
            bodyResponsibilities.text = BuildBulletList(job.Responsibilities);
            bodyCompensation.text     = BuildCompensation(job);
        }

        private static string FormatJobType(JobType type) => type switch
        {
            JobType.FullTime  => "Full-time",
            JobType.PartTime  => "Part-time",
            JobType.Contract  => "Contract",
            _                 => type.ToString()
        };

        private static string BuildBulletList(string[] items)
        {
            if (items == null || items.Length == 0) return string.Empty;
            var sb = new StringBuilder();
            foreach (var item in items)
                sb.AppendLine($"• {item}");
            return sb.ToString().TrimEnd();
        }

        private static string BuildCompensation(JobData job)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"${job.BaseSalary:N0}/month");
            if (job.Benefits != null && job.Benefits.Length > 0)
            {
                sb.AppendLine();
                foreach (var benefit in job.Benefits)
                    sb.AppendLine($"• {benefit}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
