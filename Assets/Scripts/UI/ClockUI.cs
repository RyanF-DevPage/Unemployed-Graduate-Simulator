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
            GameTimeManager.Instance.OnTimeChanged += RefreshTime;
            RefreshTime(GameTimeManager.Instance.CurrentDay,
                GameTimeManager.Instance.CurrentHour,
                GameTimeManager.Instance.CurrentMinute);
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnTimeChanged -= RefreshTime;
        }

        void RefreshTime(int day, int hour, int minute)
        {
            dayText.text = $"Day {day}";
            timeText.text = $"{hour:D2}:{minute:D2}";
        }
    }
}
