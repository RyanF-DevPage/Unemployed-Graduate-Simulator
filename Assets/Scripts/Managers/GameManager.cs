using System;
using UnityEngine;

namespace Simulator_Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnGameOverTriggered;
        public event Action OnGameWinTriggered;

        private bool _gameEnded;

        #region Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        private void Start()
        {
            PlayerStatsManager.Instance.OnGameOver       += OnPlayerLose;
            PlayerStatsManager.Instance.OnBalanceChanged += CheckWinCondition;
        }

        private void OnDestroy()
        {
            if (PlayerStatsManager.Instance == null) return;
            PlayerStatsManager.Instance.OnGameOver       -= OnPlayerLose;
            PlayerStatsManager.Instance.OnBalanceChanged -= CheckWinCondition;
        }

        public void StartNewGame()
        {
            _gameEnded = false;
            ApplicationStateManager.Instance.Reset();
            EmailManager.Instance.Reset();
            if (InterviewSession.Instance != null) InterviewSession.Instance.Reset();
            PlayerStatsManager.Instance.ResetAllStats();
            GameTimeManager.Instance.StartNewGame();
        }

        public void LoadGame()
        {
            _gameEnded = false;
            PlayerStatsManager.Instance.Load();
            GameTimeManager.Instance.Load();
        }


        public void OnPlayerWin()
        {
            if (_gameEnded) return;
            _gameEnded = true;
            GameTimeManager.Instance.Pause();
            OnGameWinTriggered?.Invoke();
        }

        public void OnPlayerLose()
        {
            _gameEnded = true;
            GameTimeManager.Instance.Pause();
            OnGameOverTriggered?.Invoke();
        }

        private void CheckWinCondition(float balance)
        {
            if (balance >= 1_000_000f)
                OnPlayerWin();
        }
    }
}
