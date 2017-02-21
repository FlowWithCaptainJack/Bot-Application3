using System.Collections.Generic;

namespace Bot.model
{
    public class Customer : BotAccount
    {
        public static List<Customer> Customers = new List<Customer>();
        public bool BotEnabled { get; set; } = true;
        public Customer(string conversationId, string userId, string name, string botName, string botId, string serviceUrl) : base(conversationId, userId, name, botName, botId, serviceUrl)
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
