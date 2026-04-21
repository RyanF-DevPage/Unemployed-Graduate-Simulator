using System;
using UnityEngine;

namespace Simulator_Game
{
    public class GameTimeManager : MonoBehaviour, ISaveable
    {
        [SerializeField] private GameTime _gameTimeConfig;

        private float _totalMinute;
        private bool _isPaused;
        private bool _isInEvent;

        public event Action<int, int, int> OnTimeChanged;

        private const string SaveTimeKey = "GameTotalMinutes";

        public int CurrentDay => Mathf.FloorToInt(_totalMinute / (24 * 60));
        public int CurrentHour => Mathf.FloorToInt(_totalMinute / 60) % 24;
        public int CurrentMinute => Mathf.FloorToInt(_totalMinute % 60);
        public float TotalMinutes => _totalMinute;

        #region Singleton
        public static GameTimeManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        #endregion

        void Update()
        {
            if (_isPaused || _isInEvent) return;

            _totalMinute += _gameTimeConfig.minutePerRealSecond * Time.deltaTime;
            EventNotifier();
        }

        public void StartNewGame()
        {
            PlayerPrefs.DeleteKey(SaveTimeKey);
            _totalMinute = _gameTimeConfig.DefaultStartInMinutes;
            EventNotifier();
        }

        public void SkipMinutes(float minutes)
        {
            _totalMinute += minutes;
            EventNotifier();
        }

        public void BeginEvent() => _isInEvent = true;
        public void EndEvent() => _isInEvent = false;

        public void Pause() => _isPaused = true;
        public void Resume() => _isPaused = false;

        /// <summary>Kept for backwards compatibility — delegates to Load().</summary>
        public void LoadGame() => Load();

        #region ISaveable
        public void Save()
        {
            PlayerPrefs.SetFloat(SaveTimeKey, _totalMinute);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            _totalMinute = PlayerPrefs.HasKey(SaveTimeKey)
                ? PlayerPrefs.GetFloat(SaveTimeKey)
                : _gameTimeConfig.DefaultStartInMinutes;
            EventNotifier();
        }
        #endregion

        private int _lastNotifiedMinute = -1;

        private void EventNotifier()
        {
            int day = CurrentDay;
            int hour = CurrentHour;
            int minute = CurrentMinute;
            if (minute == _lastNotifiedMinute) return;
            _lastNotifiedMinute = minute;
            OnTimeChanged?.Invoke(day, hour, minute);
        }
    }
}
