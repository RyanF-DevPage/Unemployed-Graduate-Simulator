using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private Button  button;
        [SerializeField] private Image   icon;
        [SerializeField] private Sprite  pauseSprite;
        [SerializeField] private Sprite  resumeSprite;

        private void Awake() => button.onClick.AddListener(Toggle);

        private void Toggle()
        {
            if (GameTimeManager.Instance.IsPaused) GameTimeManager.Instance.Resume();
            else                                   GameTimeManager.Instance.Pause();

            icon.sprite = GameTimeManager.Instance.IsPaused ? resumeSprite : pauseSprite;
        }
    }
}
