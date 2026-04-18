using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simulator_Game
{ 
    public class SceneChanger : MonoBehaviour
    {
        public static SceneChanger Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

    }
} 