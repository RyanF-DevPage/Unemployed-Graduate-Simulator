using UnityEngine;

namespace Simulator_Game
{
    public class PlayerStatsManager : MonoBehaviour, ITimeDependent, ISaveable
    {

        #region Singleton
        public static PlayerStatsManager Instance { get; private set; }

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
        public void RetrievePlayerStats()
        {

        }

        public void OnTimeUpdated(int day, int hour, int minute)
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }
    }
}
