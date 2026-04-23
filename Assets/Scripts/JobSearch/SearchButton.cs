using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    [RequireComponent(typeof(Button))]
    public class SearchButton : MonoBehaviour
    {
        [SerializeField] private JobBulletinPanelUI bulletinPanel;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            int level = PlayerStatsManager.Instance != null ? PlayerStatsManager.Instance.Level : 1;
            bulletinPanel.Refresh(level);
        }
    }
}
