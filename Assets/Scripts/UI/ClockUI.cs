using TMPro;
using UnityEngine;

namespace Simulator_Game
{
    /// <summary>
    /// Renders the current in-game day and time on screen.
    /// Implements ITimeDependent so it reacts to every minute tick from GameTimeManager.
    /// </summary>
    public class ClockUI : MonoBehaviour, ITimeDependent
    {
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text timeText;

        private void OnEnable()
        {
            GameTimeManager.Instance.OnTimeChanged += OnTimeUpdated;
            OnTimeUpdated(
                GameTimeManager.Instance.CurrentDay,
                GameTimeManager.Instance.CurrentHour,
                GameTimeManager.Instance.CurrentMinute);
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnTimeChanged -= OnTimeUpdated;
        }

        public void OnTimeUpdated(int day, int hour, int minute)
        {
            dayText.text = $"Day {day}";
            timeText.text = $"{hour:D2}:{minute:D2}";
        }
    }
}
