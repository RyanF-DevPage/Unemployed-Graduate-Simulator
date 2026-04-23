using System.Collections.Generic;
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

        private readonly Dictionary<JobData, JobApplication> _applications = new();
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
            if (!_applications.TryGetValue(_currentJob, out var application))
            {
                application = new JobApplication(_currentJob);
                _applications[_currentJob] = application;
            }

            bool canApply = application.Status == ApplicationStatus.NotApplied;
            applyButton.interactable = canApply;
            applyButtonText.text     = canApply ? "Apply" : application.Status.ToString();
        }

        private void OnApplyClicked() => confirmationPopup.SetActive(true);

        private void OnConfirmYes()
        {
            _applications[_currentJob].Apply();
            confirmationPopup.SetActive(false);
            RefreshApplyButton();
        }

        private void OnConfirmNo() => confirmationPopup.SetActive(false);
    }
}
