using System.Collections.Generic;

namespace Bot.model
{
    public abstract class BotAccount
    {
        public string ConversationId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string BotName { get; set; }
        public string ServiceUrl { get; set; }
        public string BotId { get; set; }
        public BotAccount(string conversationId, string userId, string name, string botName, string botId, string serviceUrl)
        {
            ConversationId = conversationId;
            UserId = userId;
            Name = name;
            BotName = botName;
            BotId = botId;
            ServiceUrl = serviceUrl;
        }
    }
    public class BotAccountComparer : IEqualityComparer<BotAccount>
    {
        public bool Equals(BotAccount x, BotAccount y)
        {
            return x.ConversationId == y.ConversationId;
        }

        public int GetHashCode(BotAccount obj)
        {
            return obj.ConversationId.GetHashCode();
        }
    }
}
