using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simulator_Game
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnNewGameClicked()
        {
            GameManager.Instance.StartNewGame();
            SceneManager.LoadScene("Scenes/GameScene");
        }

        public void OnContinueClicked()
        {
            GameManager.Instance.LoadGame();
            SceneManager.LoadScene("Scenes/GameScene");
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
