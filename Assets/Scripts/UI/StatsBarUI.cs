using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class StatsBarUI : MonoBehaviour
    {
        [SerializeField] private Image barImage;
        [SerializeField] private PlayerStat playerStat;

        private void OnEnable()
        {
            if (playerStat != null)
            {
                playerStat.OnStatValueChanged.AddListener(UpdateBar);
            }
        }

        private void OnDisable()
        {
            if (playerStat != null)
            {
                playerStat.OnStatValueChanged.RemoveListener(UpdateBar);
            }
        }

        private void OnDestroy()
        {
            if (playerStat != null)
                playerStat.OnStatValueChanged.RemoveListener(UpdateBar);
        }

        public void UpdateBar(float normalizedValue)
        {
            if (barImage != null)
            {
                barImage.fillAmount = Mathf.Clamp01(normalizedValue);
            }
            else
            {
                Debug.LogWarning("Bar Image reference is missing in StatsBarUI.");
            }
        }
    }
}
