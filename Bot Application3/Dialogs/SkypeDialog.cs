using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot_Application3.model;
using BotApplicationDemo.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Bot_Application3.Dialogs
{
    [Serializable]
    public class SkypeDialog : BaseDialog
    {
        static Queue<string> userMessage = new Queue<string>();
        public override async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;
            if (userMessage.Count > 1000)
            {
                userMessage.Dequeue();
            }
            if (userMessage.Contains(message.From.Id + message.Id))
            {
                context.Wait(MessageReceivedAsync);
                return;
            }
            userMessage.Enqueue(message.From.Id + message.Id);
            if (message.Text == "i am a superman")
            {
                await context.PostAsync($"successfully that  you are a superman details:{JsonConvert.SerializeObject(message)}");
                if (CustomerServer.servers.Count(m => m.UserId == message.From.Id) > 0)
                {
                    context.Wait(MessageReceivedAsync);
                    return;
                }
                CustomerServer.servers.Add(new CustomerServer(message.Conversation.Id, message.From.Id, message.From.Name, message.Recipient.Name, message.Recipient.Id, message.ServiceUrl));
            }
            else
            {
                if (CustomerServer.mapping.Count(m => m.Value.ConversationId == message.Conversation.Id) > 0)
                {
                    //send to customer
                    var user = CustomerServer.mapping.FirstOrDefault(m => m.Value.ConversationId == message.Conversation.Id).Key;
                    string serviceUrl = user.ServiceUrl;
                    var userAccount = new ChannelAccount(id: user.UserId);
                    var botAccount = new ChannelAccount(id: user.BotId, name: user.BotName);
                    var conversationId = user.ConversationId;
                    await BotUtil.SendActivityFromBot(conversationId, userAccount, botAccount, $"【{message.From.Name}】:{message.Text}", serviceUrl);
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
