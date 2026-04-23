using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class JobPostingPanelUI : MonoBehaviour
    {
        [SerializeField] private JobPostingPageUI pageUI;
        [SerializeField] private Button           applyButton;
        [SerializeField] private TMP_Text         applyButtonText;
        [SerializeField] private GameObject       confirmationPopup;
        [SerializeField] private Button           yesButton;
        [SerializeField] private Button           noButton;

        private JobData _currentJob;

        private void Awake()
        {
            applyButton.onClick.AddListener(OnApplyClicked);
            yesButton.onClick.AddListener(OnConfirmYes);
            noButton.onClick.AddListener(OnConfirmNo);
        }

        public void Show(JobData job)
        {
            _currentJob = job;
            gameObject.SetActive(true);
            pageUI.Populate(job);
            RefreshApplyButton();
        }

        public void Hide()
        {
            confirmationPopup.SetActive(false);
            gameObject.SetActive(false);
        }

        private void RefreshApplyButton()
        {
            var status = ApplicationStateManager.Instance.GetStatus(_currentJob);
            bool canApply = status == ApplicationStatus.NotApplied;
            applyButton.interactable = canApply;
            applyButtonText.text     = canApply ? "Apply" : status.ToString();
        }

        private void OnApplyClicked() => confirmationPopup.SetActive(true);

        private void OnConfirmYes()
        {
            ApplicationStateManager.Instance.TryApply(_currentJob);
            confirmationPopup.SetActive(false);
            RefreshApplyButton();
        }

        private void OnConfirmNo() => confirmationPopup.SetActive(false);
    }
}
