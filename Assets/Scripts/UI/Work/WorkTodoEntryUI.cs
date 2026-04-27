using TMPro;
using UnityEngine;

namespace Simulator_Game
{
    public class WorkTodoEntryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text timeText;

        public JobData Job { get; private set; }

        public void Setup(JobData job)
        {
            Job = job;
            titleText.text = job.JobTitle;
            timeText.text  = $"{job.workStartHour:D2}:00 - {job.workEndHour:D2}:00";
        }
    }
}
