using System;
using System.IO;
using UnityEngine;

namespace Simulator_Game
{
    public class PlayerStatsManager : MonoBehaviour, ITimeDependent, ISaveable, ILevelable, IWallet
    {
        [SerializeField] private PlayerData data;

        [Header("Depletion Rates (per game minute)")]
        [SerializeField] private float moodDepletionRate           = 1f;
        [SerializeField] private float hungerDepletionRate         = 1f;
        [SerializeField] private float healthCriticalDepletionRate = 2f;

        private const string SavePath = "/playerdata.json";

        // ── Events ──────────────────────────────────────────────────────────
        public event Action<float> OnHealthChanged;
        public event Action<float> OnMoodChanged;
        public event Action<float> OnHungerChanged;
        public event Action<float> OnBalanceChanged;
        public event Action<int>   OnLevelUp;
        public event Action        OnGameOver;

        // ── Singleton ────────────────────────────────────────────────────────
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

        private void Start()
        {
            GameTimeManager.Instance.OnTimeChanged += OnTimeUpdated;
        }

        private void OnDestroy()
        {
            if (GameTimeManager.Instance != null)
                GameTimeManager.Instance.OnTimeChanged -= OnTimeUpdated;
        }

        // ── ITimeDependent ───────────────────────────────────────────────────
        public void OnTimeUpdated(int day, int hour, int minute)
        {
            SetHunger(data.hunger - hungerDepletionRate);
            SetMood(data.mood - moodDepletionRate);

            bool isCritical = data.hunger < 25f || data.mood < 25f;
            if (isCritical)
                SetHealth(data.health - healthCriticalDepletionRate);

            if (data.health <= 0f || data.hunger <= 0f || data.mood <= 0f)
                OnGameOver?.Invoke();
        }

        // ── Vitals ───────────────────────────────────────────────────────────
        public void ModifyHealth(float delta) => SetHealth(data.health + delta);
        public void SetHealth(float value)
        {
            data.health = Mathf.Clamp(value, 0f, 100f);
            OnHealthChanged?.Invoke(data.health);
        }

        public void ModifyMood(float delta) => SetMood(data.mood + delta);
        public void SetMood(float value)
        {
            data.mood = Mathf.Clamp(value, 0f, 100f);
            OnMoodChanged?.Invoke(data.mood);
        }

        public void ModifyHunger(float delta) => SetHunger(data.hunger + delta);
        public void SetHunger(float value)
        {
            data.hunger = Mathf.Clamp(value, 0f, 100f);
            OnHungerChanged?.Invoke(data.hunger);
        }

        // ── ILevelable ───────────────────────────────────────────────────────
        public int   Level         => data.level;
        public float CurrentXP     => data.currentXP;
        public float XPToNextLevel => data.xpToNextLevel;

        public void AddXP(float amount)
        {
            data.currentXP += amount;
            while (data.currentXP >= data.xpToNextLevel)
            {
                data.currentXP    -= data.xpToNextLevel;
                data.level        += 1;
                data.skillPoints  += 1;
                data.xpToNextLevel = 100f * Mathf.Pow(1.25f, data.level - 1);
                OnLevelUp?.Invoke(data.level);
            }
        }

        public void SpendSkillPoint()
        {
            if (data.skillPoints > 0)
                data.skillPoints--;
        }

        // ── IWallet ──────────────────────────────────────────────────────────
        public float Balance => data.walletBalance;

        public void AddFunds(float amount)
        {
            data.walletBalance = Mathf.Max(0f, data.walletBalance + amount);
            OnBalanceChanged?.Invoke(data.walletBalance);
        }

        public bool TrySpend(float amount)
        {
            if (data.walletBalance < amount) return false;
            data.walletBalance -= amount;
            OnBalanceChanged?.Invoke(data.walletBalance);
            return true;
        }

        // ── Read-only accessors ──────────────────────────────────────────────
        public float Health             => data.health;
        public float Mood               => data.mood;
        public float Hunger             => data.hunger;
        public int   SkillPoints        => data.skillPoints;
        public float HungerDepletionRate => hungerDepletionRate;

        // ── Reset ────────────────────────────────────────────────────────────
        public void ResetAllStats()
        {
            data.ResetToDefaults();
            OnHealthChanged?.Invoke(data.health);
            OnMoodChanged?.Invoke(data.mood);
            OnHungerChanged?.Invoke(data.hunger);
            OnBalanceChanged?.Invoke(data.walletBalance);
        }

        // ── ISaveable ────────────────────────────────────────────────────────
        public void Save()
        {
            File.WriteAllText(Application.persistentDataPath + SavePath, JsonUtility.ToJson(data));
        }

        public void Load()
        {
            string path = Application.persistentDataPath + SavePath;
            if (File.Exists(path))
                JsonUtility.FromJsonOverwrite(File.ReadAllText(path), data);
            else
                data.ResetToDefaults();

            OnHealthChanged?.Invoke(data.health);
            OnMoodChanged?.Invoke(data.mood);
            OnHungerChanged?.Invoke(data.hunger);
            OnBalanceChanged?.Invoke(data.walletBalance);
        }
    }
}
