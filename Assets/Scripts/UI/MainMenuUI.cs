using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simulator_Game
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnNewGameClicked()
        {
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
