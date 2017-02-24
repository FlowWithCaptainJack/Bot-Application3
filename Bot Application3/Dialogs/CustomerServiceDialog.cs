﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Bot_Application3.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    class CustomerServiceDialog : BaseDialog
    {
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;
            using (var db = new BotdbUtil())
            {
                var customerServer = db.CustomerServer.Where(m => m.UserId == message.From.Id && m.Customer != null).FirstOrDefault();
                if (customerServer != null)
                {
                    // stop chat with current customer?
                    if (message.Text.ToLower() == "#exit#")
                    {
                        await SendActivity(customerServer.Customer, $"已经断开与客服【{customerServer.Name}】的连接");
                        await SendActivity(customerServer, $"已经断开与客户【{customerServer.Customer.Name}】的连接");
                        customerServer.Customer = null;
                        db.SaveChanges();
                        context.Wait(MessageReceivedAsync);
                        return;
                    }
                    //send to customer
                    await SendActivity(customerServer.Customer, $"【{message.From.Name}】:{message.Text}");
                }
                else
                {
                    await context.PostAsync($"no user need help");
                }
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}
