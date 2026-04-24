using System;
using System.Collections.Generic;

namespace Simulator_Game
{
    public interface IEmailService
    {
        IReadOnlyList<Email> Emails { get; }
        int UnreadCount { get; }
        void MarkRead(Email email);
        event Action OnMailboxChanged;
    }
}
