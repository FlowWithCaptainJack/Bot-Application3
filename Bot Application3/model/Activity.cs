using System;
using System.Collections.Generic;

namespace BotApplicationDemo.model
{
    public class Activity
    {
        public string id { get; set; }
        public string type { get; set; }

        public string text { get; set; }

        public string replyToId { get; set; }

        public From from { get; set; }

        public DateTime timestamp { get; set; }

        public Recipient recipient { get; set; }

        public Conversation conversation { get; set; }
        public class From
        {
            public string id { get; set; }

            public string name { get; set; }
        }

        public class Recipient
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Conversation
        {
            public string id { get; set; }
        }

    }

    public class BotActivity
    {
        public List<Activity> activities { get; set; }
        public string watermark { get; set; }
    }
}
