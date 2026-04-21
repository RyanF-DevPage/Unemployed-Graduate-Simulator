using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public enum StatType { Health, Mood, Hunger }

    public class StatsBarUI : MonoBehaviour
    {
        [SerializeField] private Image    barImage;
        [SerializeField] private StatType statType;

        private void OnEnable()
        {
            var mgr = PlayerStatsManager.Instance;
            if (mgr == null) return;

            switch (statType)
            {
                case StatType.Health: mgr.OnHealthChanged += UpdateBar; UpdateBar(mgr.Health); break;
                case StatType.Mood:   mgr.OnMoodChanged   += UpdateBar; UpdateBar(mgr.Mood);   break;
                case StatType.Hunger: mgr.OnHungerChanged += UpdateBar; UpdateBar(mgr.Hunger); break;
            }
        }

        private void OnDisable()
        {
            var mgr = PlayerStatsManager.Instance;
            if (mgr == null) return;

            switch (statType)
            {
                case StatType.Health: mgr.OnHealthChanged -= UpdateBar; break;
                case StatType.Mood:   mgr.OnMoodChanged   -= UpdateBar; break;
                case StatType.Hunger: mgr.OnHungerChanged -= UpdateBar; break;
            }
        }

        private void UpdateBar(float value)
        {
            if (barImage != null)
                barImage.fillAmount = Mathf.Clamp01(value / 100f);
        }
    }
}
