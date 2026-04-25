using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class IntroPanelUI : MonoBehaviour
    {
        [SerializeField] private Button dismissButton;

        private void OnEnable()
        {
            dismissButton.onClick.AddListener(GameManager.Instance.StartNewGame);
        }

        private void OnDisable()
        {
            dismissButton.onClick.RemoveListener(GameManager.Instance.StartNewGame);
        }
    }
}
