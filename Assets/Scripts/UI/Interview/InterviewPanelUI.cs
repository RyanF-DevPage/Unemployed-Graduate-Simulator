using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class InterviewPanelUI : MonoBehaviour
    {
        [Header("Question stage")]
        [SerializeField] private GameObject questionView;
        [SerializeField] private TMP_Text   questionText;
        [SerializeField] private Button[]   answerButtons;
        [SerializeField] private TMP_Text[] answerTexts;

        [Header("Thank-you stage")]
        [SerializeField] private GameObject thankYouView;
        [SerializeField] private Button     thankYouCloseButton;

        private void Awake()
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                int idx = i;
                answerButtons[i].onClick.AddListener(() => InterviewSession.Instance.SelectAnswer(idx));
            }
            thankYouCloseButton.onClick.AddListener(() => InterviewSession.Instance.Close());

            // Inner views start hidden; the root stays active so events still fire.
            questionView.SetActive(false);
            thankYouView.SetActive(false);

            // Root Image (a transparent placeholder) must not block raycasts when idle.
            var rootImg = GetComponent<Image>();
            if (rootImg != null) rootImg.raycastTarget = false;
        }

        private void OnEnable()
        {
            if (InterviewSession.Instance == null) return;
            InterviewSession.Instance.OnStageStarted      += ShowQuestion;
            InterviewSession.Instance.OnInterviewComplete += ShowThankYou;
            InterviewSession.Instance.OnInterviewClosed   += Hide;
        }

        private void OnDisable()
        {
            if (InterviewSession.Instance == null) return;
            InterviewSession.Instance.OnStageStarted      -= ShowQuestion;
            InterviewSession.Instance.OnInterviewComplete -= ShowThankYou;
            InterviewSession.Instance.OnInterviewClosed   -= Hide;
        }

        private void ShowQuestion(InterviewQuestion question, List<InterviewAnswer> answers)
        {
            questionView.SetActive(true);
            thankYouView.SetActive(false);

            questionText.text = question.questionText;
            for (int i = 0; i < answerButtons.Length; i++)
            {
                bool has = i < answers.Count;
                answerButtons[i].gameObject.SetActive(has);
                if (has) answerTexts[i].text = answers[i].answerText;
            }
        }

        private void ShowThankYou()
        {
            questionView.SetActive(false);
            thankYouView.SetActive(true);
        }

        private void Hide()
        {
            questionView.SetActive(false);
            thankYouView.SetActive(false);
        }
    }
}
