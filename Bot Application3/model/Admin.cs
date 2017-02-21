﻿using System.Collections.Generic;

namespace Bot.model
{
    public class Admin : BotAccount
    {
        public Admin(string conversationId, string userId, string name, string botName, string botId, string serviceUrl) : base(conversationId, userId, name, botName, botId, serviceUrl)
        {
        }
        public static List<Admin> Admins = new List<Admin>();
        public static Dictionary<Customer, Admin> mapping = new Dictionary<Customer, Admin>(new BotAccountComparer());
    }
}
