using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class EmailDetailPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text senderText;
        [SerializeField] private TMP_Text receiverText;
        [SerializeField] private TMP_Text contentText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button startInterviewButton;
        [SerializeField] private Button acceptOfferButton;
        [SerializeField] private Button rejectOfferButton;
        [SerializeField] private GameObject workConflictWarningPanel;
        [SerializeField] private Button conflictWarningOkButton;

        private Email _currentEmail;

        private void Awake()
        {
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
            if (startInterviewButton != null)
                startInterviewButton.onClick.AddListener(OnStartInterviewClicked);
            if (acceptOfferButton != null)
                acceptOfferButton.onClick.AddListener(OnAcceptOfferClicked);
            if (rejectOfferButton != null)
                rejectOfferButton.onClick.AddListener(OnRejectOfferClicked);
            if (conflictWarningOkButton != null)
                conflictWarningOkButton.onClick.AddListener(() => workConflictWarningPanel.SetActive(false));
        }

        public void Show(Email email)
        {
            _currentEmail     = email;
            titleText.text    = email.title;
            senderText.text   = $"From: {email.sender}";
            receiverText.text = $"To: {email.receiver}";
            contentText.text  = email.content;

            if (startInterviewButton != null)
                startInterviewButton.gameObject.SetActive(
                    email.relatedStatus == ApplicationStatus.Interview && email.job != null);

            bool isActiveOffer = email.relatedStatus == ApplicationStatus.OfferReceived
                && email.job != null
                && ApplicationStateManager.Instance.GetStatus(email.job) == ApplicationStatus.OfferReceived;

            if (acceptOfferButton != null) acceptOfferButton.gameObject.SetActive(isActiveOffer);
            if (rejectOfferButton != null) rejectOfferButton.gameObject.SetActive(isActiveOffer);
            if (workConflictWarningPanel != null) workConflictWarningPanel.SetActive(false);

            gameObject.SetActive(true);
        }

        private void OnStartInterviewClicked()
        {
            if (_currentEmail == null || _currentEmail.job == null) return;
            gameObject.SetActive(false);
            InterviewSession.Instance.StartInterview(_currentEmail.job);
        }

        private void OnAcceptOfferClicked()
        {
            if (_currentEmail?.job == null) return;
            if (ApplicationStateManager.Instance.AcceptOffer(_currentEmail.job))
                gameObject.SetActive(false);
            else if (workConflictWarningPanel != null)
                workConflictWarningPanel.SetActive(true);
        }

        private void OnRejectOfferClicked()
        {
            if (_currentEmail?.job == null) return;
            ApplicationStateManager.Instance.RejectOffer(_currentEmail.job);
            gameObject.SetActive(false);
        }
    }
}
