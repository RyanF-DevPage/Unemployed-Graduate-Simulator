using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class EmailEntryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text senderText;
        [SerializeField] private GameObject unreadDot;
        [SerializeField] private Button button;

        public void Setup(Email email, Action<Email> onClick)
        {
            titleText.text  = email.title;
            senderText.text = email.sender;
            unreadDot.SetActive(!email.isRead);

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick(email));
        }
    }
}
