using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class WorkUI : MonoBehaviour
    {
        [Header("Prompt Panel")]
        [SerializeField] private GameObject promptPanel;
        [SerializeField] private Button     letsGoButton;

        [Header("Loading Panel")]
        [SerializeField] private GameObject loadingPanel;

        [SerializeField] private float loadingDurationSeconds = 5f;

        private JobData _currentJob;

        private void Start()
        {
            WorkManager.Instance.OnWorkTimeReached += ShowPrompt;
            letsGoButton.onClick.AddListener(OnLetsGoClicked);
        }

        private void OnDestroy()
        {
            if (WorkManager.Instance != null)
                WorkManager.Instance.OnWorkTimeReached -= ShowPrompt;
        }

        private void ShowPrompt(JobData job)
        {
            _currentJob = job;
            promptPanel.SetActive(true);
        }

        private void OnLetsGoClicked()
        {
            promptPanel.SetActive(false);
            loadingPanel.SetActive(true);
            StartCoroutine(WorkRoutine());
        }

        private IEnumerator WorkRoutine()
        {
            yield return new WaitForSeconds(loadingDurationSeconds);
            loadingPanel.SetActive(false);
            WorkManager.Instance.CompleteWork(_currentJob);
        }
    }
}
