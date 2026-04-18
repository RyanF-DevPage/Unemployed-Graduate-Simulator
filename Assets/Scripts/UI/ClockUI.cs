using TMPro;
using UnityEngine;

namespace Simulator_Game
{
    public class ClockUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text timeText;

        private void OnEnable()
        {
            GameTimeManager.Instance.OnTimeChanged += RefreshTime; // Subscribe to time change events when the UI is enabled
        }
        private void OnDisable()
        {
            GameTimeManager.Instance.OnTimeChanged -= RefreshTime; // Unsubscribe from time change events when the UI is disabled to prevent memory leaks
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            RefreshTime(GameTimeManager.Instance.CurrentDay,
                GameTimeManager.Instance.CurrentHour,
                GameTimeManager.Instance.CurrentMinute); // Initialize the clock UI with the current time
        }

        void RefreshTime(int day, int hour, int minute)
        {
            dayText.text = $"Day {day}"; // Display the current day
            timeText.text = $"{hour:D2}:{minute:D2}"; // Display the current time in HH:MM format
        }
    }
}
