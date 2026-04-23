using UnityEngine;

namespace Simulator_Game
{
    public class JobPostingPanelUI : MonoBehaviour
    {
        [SerializeField] private JobPostingPageUI pageUI;

        public void Show(JobData job)
        {
            gameObject.SetActive(true);
            pageUI.Populate(job);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
