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

        private Email _currentEmail;

        private void Awake()
        {
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
            if (startInterviewButton != null)
                startInterviewButton.onClick.AddListener(OnStartInterviewClicked);
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

            gameObject.SetActive(true);
        }

        private void OnStartInterviewClicked()
        {
            if (_currentEmail == null || _currentEmail.job == null) return;
            gameObject.SetActive(false);
            InterviewSession.Instance.StartInterview(_currentEmail.job);
        }
    }
}
