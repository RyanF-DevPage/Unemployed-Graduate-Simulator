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

        public void UpdateBar(float normalizedValue)
        {
            if (barImage != null)
            {
                barImage.fillAmount = Mathf.Clamp01(normalizedValue); 
                //Clamp might not be needed here, but used as a safety measure to ensure fillAmount stays between 0 and 1
            }
            else
            {
                Debug.LogWarning("Bar Image reference is missing in StatsBarUI.");
            }
        }
    }
}
