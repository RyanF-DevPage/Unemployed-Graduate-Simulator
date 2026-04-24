using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class EmailDetailPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text senderText;
        [SerializeField] private TMP_Text receiverText;
        [SerializeField] private TMP_Text contentText;
        [SerializeField] private Button closeButton;

        private void Awake() => closeButton.onClick.AddListener(() => gameObject.SetActive(false));

        public void Show(Email email)
        {
            titleText.text    = email.title;
            senderText.text   = $"From: {email.sender}";
            receiverText.text = $"To: {email.receiver}";
            contentText.text  = email.content;
            gameObject.SetActive(true);
        }
    }
}
