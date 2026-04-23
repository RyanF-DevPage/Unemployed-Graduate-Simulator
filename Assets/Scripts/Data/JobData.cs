using UnityEngine;

namespace Simulator_Game
{
    [CreateAssetMenu(fileName = "NewJob", menuName = "Game/Job")]
    public class JobData : ScriptableObject, IJob
    {
        [Header("Listing")]
        public string jobTitle;
        public string companyName;
        public JobType jobType;

        [Header("Requirements")]
        public int requiredLevel = 1;
        [TextArea] public string[] requirements;
        [TextArea] public string[] responsibilities;

        [Header("Compensation")]
        public float baseSalary;
        public float workDurationHours;
        [TextArea] public string[] benefits;

        public string JobTitle           => jobTitle;
        public string CompanyName        => companyName;
        public JobType JobType           => jobType;
        public float BaseSalary          => baseSalary;
        public int RequiredLevel         => requiredLevel;
        public float WorkDurationHours => workDurationHours;
        public string[] Requirements     => requirements;
        public string[] Responsibilities => responsibilities;
        public string[] Benefits         => benefits;

        public bool IsEligible(int playerLevel) => playerLevel >= requiredLevel;
    }
}
