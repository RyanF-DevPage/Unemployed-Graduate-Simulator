using TMPro;
using UnityEngine;

namespace Simulator_Game
{
    public class BalanceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text       valueText;
        private void Start()
        {
            PlayerStatsManager.Instance.OnBalanceChanged += Refresh;
            Refresh(PlayerStatsManager.Instance.Balance);
        }

        private void OnDestroy()
        {
            if (PlayerStatsManager.Instance != null)
                PlayerStatsManager.Instance.OnBalanceChanged -= Refresh;
        }

        private void Refresh(float balance)
        {
            valueText.text = $"{balance:N0}";
            valueText.ForceMeshUpdate();
        }
    }
}
