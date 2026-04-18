using System;
using UnityEngine;

namespace Simulator_Game
{
    public class GameTimeManager : MonoBehaviour
    {
        [SerializeField] private GameTime _gameTimeConfig; // Reference to the GameTime ScriptableObject

        // Game state variables
        private float _totalMinute;
        private bool _isPaused;
        private bool _isInEven;

        // Event to notify listeners of time changes (hour, minute, day)
        public event Action<int, int, int> OnTimeChanged;

        // // Key for saving time in PlayerPrefs
        private const string SaveTimeKey = "GameTotalMinutes";

        // getters for current time components
        public int CurrentDay => Mathf.FloorToInt(_totalMinute / (24 * 60)); // Calculate current day from total minutes
        public int CurrentHour => Mathf.FloorToInt(_totalMinute / 60) % 24; // Calculate current hour from total minutes
        public int CurrentMinute => Mathf.FloorToInt(_totalMinute % 60); // Calculate current minute from total minutes
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


        // Update is called once per frame
        void Update()
        {
            if (_isPaused || _isInEven)  return; // Pause time progression if the game is paused (e.g. in settings) or in an event

            _totalMinute += _gameTimeConfig.minutePerRealSecond * Time.deltaTime; // Increment total minutes based on time scale
            EventNotifier();
        }

        /// <summary>
        /// Starts a new game by resetting the total minutes and invoking the time change event to update any listeners (e.g. UI).
        /// </summary>
        public void StartNewGame()
        {
            PlayerPrefs.DeleteKey(SaveTimeKey); // Clear saved time for a new game
            _totalMinute = _gameTimeConfig.DefaultStartInMinutes;
            EventNotifier();
        }

        /// <summary>
        /// Skil Minutes is called during an event or activity (e.g.working,sleeping) to advance time by a specified number of minutes.
        /// It also ensures that time progression is paused during the event and resumes afterward.
        /// </summary>
        /// <param name="minutes"></param>
        public void SkipMinutes(float minutes)
        {
            _isInEven = false; // Ensure we are not in an event when skipping time
            _totalMinute += minutes;
            EventNotifier();
        }

        public void BeginEvent() => _isInEven = true; // Set event flag to pause time progression during events
        public void EndEvent() => _isInEven = false; // Clear event flag to resume time progression after events


        /// <summary>
        /// LoadGame is called when the player wants to continue a previously saved game. 
        /// It retrieves the total minutes from PlayerPrefs and updates the current time accordingly. 
        /// If no saved time exists, it defaults to the starting time defined in the GameTime ScriptableObject. 
        /// After loading, it invokes the time change event to update any listeners (e.g. UI).
        /// </summary>
        public void LoadGame()
        {
            _totalMinute = PlayerPrefs.HasKey(SaveTimeKey) 
                ? PlayerPrefs.GetFloat(SaveTimeKey) 
                : _gameTimeConfig.DefaultStartInMinutes; // Load saved time or start time if no save exists
            EventNotifier();
        }

        private float _lastNotifiedMinute = -1;

        /// <summary>
        /// A private method to invoke the OnTimeChanged event whenever the minute changes.
        /// It ensures that listeners are only notified when the minute changes to prevent unnecessary updates (e.g. UI updates) every frame.
        /// </summary>
        private void EventNotifier()
        {
            int minute = CurrentMinute;
            if (minute == _lastNotifiedMinute) return;
            _lastNotifiedMinute = minute;
            
            OnTimeChanged?.Invoke(CurrentDay, CurrentHour, CurrentMinute); // Notify listeners of time changes
        }
    }
}
