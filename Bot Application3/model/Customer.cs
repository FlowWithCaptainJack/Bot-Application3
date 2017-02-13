using System.Collections.Generic;

namespace Bot_Application3.model
{
    public class Customer
    {
        public string ConversationId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public string BotName { get; set; }
        public string ServiceUrl { get; set; }
        public string BotId { get; set; }
        public static List<Customer> Customers = new List<Customer>();

        public Customer(string conversationId, string userId, string name, string botName, string botId, string serviceUrl)
        {
            ConversationId = conversationId;
            UserId = userId;
            Name = name;
            BotName = botName;
            BotId = botId;
            ServiceUrl = serviceUrl;
        }
    }
}
