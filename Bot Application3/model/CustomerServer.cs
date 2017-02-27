﻿using System.Collections.Generic;

namespace Bot.model
{
    public class CustomerServer : BotAccount
    {
        public CustomerServer()
        {
        }
        public CustomerServer(string conversationId, string userId, string name, string botName, string botId, string serviceUrl) : base(conversationId, userId, name, botName, botId, serviceUrl)
        {
            ConversationId = conversationId;
            UserId = userId;
            Name = name;
            BotName = botName;
            BotId = botId;
            ServiceUrl = serviceUrl;
        }
        public virtual Customer Customer { get; set; }
    }
}
