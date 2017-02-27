using System.Collections.Generic;

namespace Bot.model
{
    public class Admin : BotAccount
    {
        public Admin()
        {

        }
        public Admin(string conversationId, string userId, string name, string botName, string botId, string serviceUrl) : base(conversationId, userId, name, botName, botId, serviceUrl)
        {
        }
        public virtual Customer Customer { get; set; }
    }
}
