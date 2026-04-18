using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simulator_Game
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnNewGameClicked()
        {
            GameTimeManager.Instance.StartNewGame();
            SceneManager.LoadScene("Scenes/GameScene");
        }

        public void OnContinueClicked()
        {
            GameTimeManager.Instance.LoadGame();
            SceneManager.LoadScene("Scenes/GameScene");
        }
    }
}
