using UnityEngine;

namespace Simulator_Game
{
    public class EmailIconUI : MonoBehaviour
    {
        [SerializeField] private GameObject notificationDot;

        private void Start()
        {
            EmailManager.Instance.OnMailboxChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (EmailManager.Instance != null)
                EmailManager.Instance.OnMailboxChanged -= Refresh;
        }

        private void Refresh() => notificationDot.SetActive(EmailManager.Instance.UnreadCount > 0);
    }
}
