using UnityEngine;

namespace Simulator_Game
{
    public class GameManager : MonoBehaviour
    {

        #region Singleton
        public static GameManager Instance { get; private set; }

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

        public void StartNewGame()
        {
            
        }

        public void LoadGame()
        {

        }


    }
}
