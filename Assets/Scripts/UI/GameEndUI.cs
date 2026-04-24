using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Simulator_Game
{
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private Button gameOverMainMenuButton;
        [SerializeField] private Button winMainMenuButton;

        private void Awake()
        {
            gameOverMainMenuButton.onClick.AddListener(GoToMainMenu);
            winMainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGameOverTriggered += ShowGameOver;
            GameManager.Instance.OnGameWinTriggered  += ShowWin;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnGameOverTriggered -= ShowGameOver;
            GameManager.Instance.OnGameWinTriggered  -= ShowWin;
        }

        private void ShowGameOver() => gameOverPanel.SetActive(true);
        private void ShowWin()      => winPanel.SetActive(true);

        private void GoToMainMenu() => SceneManager.LoadScene("MainMenu");
    }
}
