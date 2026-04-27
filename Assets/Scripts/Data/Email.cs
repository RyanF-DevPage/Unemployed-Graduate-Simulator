using System;

namespace Simulator_Game
{
    [Serializable]
    public class Email
    {
        public string title;
        public string sender;
        public string receiver;
        public string content;
        public bool isRead;

        public ApplicationStatus relatedStatus;
        [NonSerialized] public JobData job;
    }
}
