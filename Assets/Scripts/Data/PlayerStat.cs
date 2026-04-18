using UnityEngine;
using UnityEngine.Events;

namespace Simulator_Game
{
    [CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/PlayerStat")]
    public class PlayerStat : ScriptableObject
    {
        [Header("Stat Information")]
        public string statName;
        public float maxValue;

        [Header("Normalized State")]
        [SerializeField,Range(0f,1f)] private float normalizedValue = 1f;

        public UnityEvent<float> OnStatValueChanged;

        public float CurrentValue
        {
            get => normalizedValue * maxValue;
            set
            {
                normalizedValue = Mathf.Clamp01(value / maxValue);
                OnStatValueChanged?.Invoke(normalizedValue);
            }
        }
        
        public float NormalizedValue => normalizedValue;

        /// <summary>
        /// Reset the stat to its default state, which is full (normalized value of 1) and current value equal to max value.
        /// </summary>
        public void ResetStat()
        {
            normalizedValue = 1f;
            OnStatValueChanged?.Invoke(normalizedValue);
        }

    }
}
