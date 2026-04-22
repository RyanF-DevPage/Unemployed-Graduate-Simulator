using UnityEngine;

namespace Simulator_Game
{
    [CreateAssetMenu(menuName = "Game/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Vitals")]
        [Range(0f, 100f)] public float health = 100f;
        [Range(0f, 100f)] public float mood   = 100f;
        [Range(0f, 100f)] public float hunger = 100f;

        [Header("Progression")]
        public int   level         = 1;
        public float currentXP    = 0f;
        public float xpToNextLevel = 100f;
        public int   skillPoints  = 0;

        [Header("Economy")]
        public float walletBalance = 0f;

        public void ResetToDefaults()
        {
            health = mood = hunger = 100f;
            level = 1; currentXP = 0f; xpToNextLevel = 100f;
            skillPoints = 0; walletBalance = 0f;
        }
    }
}
