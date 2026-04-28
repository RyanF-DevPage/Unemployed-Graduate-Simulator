using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator_Game
{
    public class EmailManager : MonoBehaviour, IEmailService
    {
        public static EmailManager Instance { get; private set; }

        public event Action OnMailboxChanged;

        private readonly List<Email> _emails = new();
        public IReadOnlyList<Email> Emails => _emails;

        public int UnreadCount
        {
            get { int n = 0; foreach (var e in _emails) if (!e.isRead) n++; return n; }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<IEmailService>(this);
        }

        private void Start()
        {
            ApplicationStateManager.Instance.OnApplicationStatusChanged += HandleStatusChange;
        }

        private void OnDestroy()
        {
            if (Instance == this) ServiceLocator.Unregister<IEmailService>();
            if (ApplicationStateManager.Instance != null)
                ApplicationStateManager.Instance.OnApplicationStatusChanged -= HandleStatusChange;
        }

        public void Reset()
        {
            _emails.Clear();
            OnMailboxChanged?.Invoke();
        }

        public void MarkRead(Email email)
        {
            if (email.isRead) return;
            email.isRead = true;
            OnMailboxChanged?.Invoke();
        }

        private void HandleStatusChange(JobData job, ApplicationStatus status)
        {
            var email = BuildEmail(job, status);
            if (email == null) return;
            email.job = job;
            email.relatedStatus = status;
            _emails.Insert(0, email);
            OnMailboxChanged?.Invoke();
        }

        private static Email BuildEmail(JobData job, ApplicationStatus status) => status switch
        {
            ApplicationStatus.Pending => new Email
            {
                title    = "Application Received",
                sender   = job.CompanyName,
                receiver = "You",
                content  = $"Thank you for applying to {job.JobTitle} at {job.CompanyName}. " +
                           "We have received your application and will be in touch shortly."
            },
            ApplicationStatus.Interview => new Email
            {
                title    = "Interview Invitation",
                sender   = job.CompanyName,
                receiver = "You",
                content  = $"We are pleased to invite you to interview for {job.JobTitle} at {job.CompanyName}. " +
                           "Please prepare for a behavioral interview."
            },
            ApplicationStatus.OfferReceived => new Email
            {
                title    = "Job Offer",
                sender   = job.CompanyName,
                receiver = "You",
                content  = $"Congratulations! We are delighted to offer you the position of {job.JobTitle} " +
                           $"at {job.CompanyName} at ${job.BaseSalary:N0}/hour. " +
                           "Please let us know whether you would like to accept or decline."
            },
            ApplicationStatus.Rejected => new Email
            {
                title    = "Application Unsuccessful",
                sender   = job.CompanyName,
                receiver = "You",
                content  = $"Thank you for your interest in {job.JobTitle} at {job.CompanyName}. " +
                           "After careful consideration, we will not be moving forward with your application."
            },
            _ => null
        };
    }
}
