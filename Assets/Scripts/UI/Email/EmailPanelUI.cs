using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator_Game
{
    public class EmailPanelUI : MonoBehaviour
    {
        [SerializeField] private Transform listContent;
        [SerializeField] private EmailEntryUI entryPrefab;
        [SerializeField] private EmailDetailPanelUI detailPanel;
        [SerializeField] private Button closeButton;

        private readonly List<EmailEntryUI> _entries = new();

        private void Awake() => closeButton.onClick.AddListener(() => gameObject.SetActive(false));

        private void Start()  => EmailManager.Instance.OnMailboxChanged += RefreshList;
        private void OnDestroy()
        {
            if (EmailManager.Instance != null)
                EmailManager.Instance.OnMailboxChanged -= RefreshList;
        }

        private void OnEnable() => RefreshList();

        private void RefreshList()
        {
            if (EmailManager.Instance == null) return;
            foreach (var e in _entries) Destroy(e.gameObject);
            _entries.Clear();

            foreach (var email in EmailManager.Instance.Emails)
            {
                var entry = Instantiate(entryPrefab, listContent);
                entry.Setup(email, OnEmailClicked);
                _entries.Add(entry);
            }
        }

        private void OnEmailClicked(Email email)
        {
            EmailManager.Instance.MarkRead(email);
            detailPanel.Show(email);
        }
    }
}
