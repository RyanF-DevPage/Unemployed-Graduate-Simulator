using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator_Game
{
    public class InterviewSession : MonoBehaviour
    {
        public static InterviewSession Instance { get; private set; }

        [SerializeField] private InterviewQuestionBank bank;
        [SerializeField] private int stageCount       = 3;
        [SerializeField] private int answersPerStage  = 3;

        public event Action<InterviewQuestion, List<InterviewAnswer>> OnStageStarted;
        public event Action OnInterviewComplete;
        public event Action OnInterviewClosed;

        private JobData _currentJob;
        private List<InterviewQuestion> _questions;
        private List<InterviewAnswer>   _currentAnswers;
        private int   _currentStage;
        private float _totalScore;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartInterview(JobData job)
        {
            if (bank == null || bank.questions == null || bank.questions.Count == 0)
            {
                Debug.LogError("[InterviewSession] No question bank assigned.");
                return;
            }

            _currentJob   = job;
            _totalScore   = 0f;
            _currentStage = 0;
            _questions    = bank.PickRandomQuestions(stageCount);

            GameTimeManager.Instance.Pause();
            StartStage(0);
        }

        public void SelectAnswer(int index)
        {
            if (_currentAnswers == null) return;
            if (index < 0 || index >= _currentAnswers.Count) return;

            _totalScore += _currentAnswers[index].scoreModifier;
            _currentStage++;

            if (_currentStage >= _questions.Count)
                OnInterviewComplete?.Invoke();
            else
                StartStage(_currentStage);
        }

        public void Close()
        {
            GameTimeManager.Instance.Resume();
            ApplicationStateManager.Instance.PostInterview(_currentJob, _totalScore);
            OnInterviewClosed?.Invoke();

            _currentJob     = null;
            _questions      = null;
            _currentAnswers = null;
        }

        public void Reset()
        {
            _currentJob     = null;
            _questions      = null;
            _currentAnswers = null;
            _currentStage   = 0;
            _totalScore     = 0f;
        }

        private void StartStage(int stage)
        {
            var question = _questions[stage];
            _currentAnswers = bank.PickRandomAnswers(question, answersPerStage);
            OnStageStarted?.Invoke(question, _currentAnswers);
        }
    }
}
