using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public enum StatType { Health, Mood, Hunger }

    public class StatsBarUI : MonoBehaviour
    {
        [SerializeField] private Image    barImage;
        [SerializeField] private StatType statType;

        private bool _started;

        // Start runs after all Awake() calls — PlayerStatsManager.Instance is guaranteed set.
        private void Start()
        {
            _started = true;
            Subscribe();
        }

        // OnEnable fires before Start on first enable, so skip until Start has run.
        // On subsequent re-enables (after a disable), re-subscribe.
        private void OnEnable()
        {
            if (_started) Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            var statsManager = PlayerStatsManager.Instance;
            if (statsManager == null)
            {
                Debug.LogWarning($"[StatsBarUI] PlayerStatsManager.Instance is null on '{gameObject.name}'");
                return;
            }

            switch (statType)
            {
                case StatType.Health: statsManager.OnHealthChanged += UpdateBar; UpdateBar(statsManager.Health); break;
                case StatType.Mood:   statsManager.OnMoodChanged   += UpdateBar; UpdateBar(statsManager.Mood);   break;
                case StatType.Hunger: statsManager.OnHungerChanged += UpdateBar; UpdateBar(statsManager.Hunger); break;
            }
        }

        private void Unsubscribe()
        {
            var statsManager = PlayerStatsManager.Instance;
            if (statsManager == null)
            {
                Debug.LogWarning($"[StatsBarUI] PlayerStatsManager.Instance is null on '{gameObject.name}'");
                return;
            }

            switch (statType)
            {
                case StatType.Health: statsManager.OnHealthChanged -= UpdateBar; break;
                case StatType.Mood:   statsManager.OnMoodChanged   -= UpdateBar; break;
                case StatType.Hunger: statsManager.OnHungerChanged -= UpdateBar; break;
            }
        }

        private void UpdateBar(float value)
        {
            if (barImage == null)
            {
                Debug.LogWarning($"[StatsBarUI] barImage is null on '{gameObject.name}'");
                return;
            }

            barImage.fillAmount = Mathf.Clamp01(value / 100f);
        }
    }
}
