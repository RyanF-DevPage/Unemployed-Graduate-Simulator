using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator_Game
{
    [CreateAssetMenu(fileName = "JobDatabase", menuName = "Game/JobDatabase")]
    public class JobDatabase : ScriptableObject
    {
        public List<JobData> jobs;

        public List<JobData> GetEligibleJobs(int playerLevel) =>
            jobs.Where(j => j.IsEligible(playerLevel)).ToList();
    }
}
