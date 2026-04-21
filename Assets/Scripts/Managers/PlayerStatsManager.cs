using UnityEngine;

namespace Simulator_Game
{
    public class PlayerStatsManager : MonoBehaviour, ITimeDependent, ISaveable
    {
        [Header("Stat References")]
        [SerializeField] private PlayerStat healthStat;
        [SerializeField] private PlayerStat hungerStat;
        [SerializeField] private PlayerStat moodStat;

        [Header("Depletion Rates (per game minute)")]
        [SerializeField] private float moodDepletionRate = -1f;
        [SerializeField] private float hungerDepletionRate = -1f;
        [SerializeField] private float healthCriticalDepletionRate = -2f;

        private const string SaveHealthKey = "Stat_Health";
        private const string SaveHungerKey = "Stat_Hunger";
        private const string SaveMoodKey   = "Stat_Mood";

        #region Singleton
        public static PlayerStatsManager Instance { get; private set; }

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

        // Start() is guaranteed to run after ALL Awake() calls across all objects,
        // so GameTimeManager.Instance is always set by this point.
        private void Start()
        {
            GameTimeManager.Instance.OnTimeChanged += OnTimeUpdated;
        }

        private void OnDestroy()
        {
            if (GameTimeManager.Instance != null)
                GameTimeManager.Instance.OnTimeChanged -= OnTimeUpdated;
        }

        // Called every in-game minute by GameTimeManager.OnTimeChanged
        public void OnTimeUpdated(int day, int hour, int minute)
        {
            if (hungerStat != null) hungerStat.CurrentValue += hungerDepletionRate;
            if (moodStat != null)   moodStat.CurrentValue   += moodDepletionRate;

            // Health depletes only when Hunger or Mood is critical (< 25%)
            if (healthStat != null)
            {
                bool isCritical = (hungerStat != null && hungerStat.NormalizedValue < 0.25f)
                               || (moodStat   != null && moodStat.NormalizedValue   < 0.25f);

                if (isCritical)
                    healthStat.CurrentValue += healthCriticalDepletionRate;
            }
        }

        #region ISaveable
        public void Save()
        {
            if (healthStat != null) PlayerPrefs.SetFloat(SaveHealthKey, healthStat.NormalizedValue);
            if (hungerStat != null) PlayerPrefs.SetFloat(SaveHungerKey, hungerStat.NormalizedValue);
            if (moodStat   != null) PlayerPrefs.SetFloat(SaveMoodKey,   moodStat.NormalizedValue);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            if (healthStat != null && PlayerPrefs.HasKey(SaveHealthKey))
                healthStat.CurrentValue = PlayerPrefs.GetFloat(SaveHealthKey) * healthStat.maxValue;

            if (hungerStat != null && PlayerPrefs.HasKey(SaveHungerKey))
                hungerStat.CurrentValue = PlayerPrefs.GetFloat(SaveHungerKey) * hungerStat.maxValue;

            if (moodStat != null && PlayerPrefs.HasKey(SaveMoodKey))
                moodStat.CurrentValue = PlayerPrefs.GetFloat(SaveMoodKey) * moodStat.maxValue;
        }
        #endregion

        public void ResetAllStats()
        {
            healthStat.ResetStat();
            hungerStat.ResetStat();
            moodStat.ResetStat();
        }
    }
}
