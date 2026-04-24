using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simulator_Game
{
    // Attach to a GameObject in a Bootstrap scene that loads before MainMenu.
    // Build order: Bootstrap (index 0), MainMenu (index 1), GameScene (index 2).
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private string firstScene = "MainMenu";

        private void Start()
        {
            SceneManager.LoadScene(firstScene);
        }
    }
}
