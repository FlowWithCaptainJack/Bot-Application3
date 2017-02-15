using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot_Application3.model;
using BotApplicationDemo.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application3.Dialogs
{
    public class CustomerDialog : BaseDialog
    {
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;
            //chat with customerService
            if (CustomerServer.mapping.Count(m => m.Key.UserId == message.From.Id) > 0)
            {
                //send to customerServer(customerRole)
                var map = CustomerServer.mapping.FirstOrDefault(m => m.Key.UserId == message.From.Id);
                var customerServer = map.Value;
                string serviceUrl = customerServer.ServiceUrl;
                var userAccount = new ChannelAccount(id: customerServer.UserId, name: customerServer.Name);
                var botAccount = new ChannelAccount(id: customerServer.BotId, name: customerServer.BotName);
                var conversationId = customerServer.ConversationId;
                await BotUtil.SendActivityFromBot(conversationId, userAccount, botAccount, $"【{message.From.Name}】:{message.Text}", serviceUrl);
                context.Wait(MessageReceivedAsync);
                return;
            }
            //connect to customerService
            if (message.Text == "9")
            {
                var busyIds = CustomerServer.mapping.Select(m => m.Value.UserId);
                var freeServers = CustomerServer.servers;
                if (busyIds?.Count() > 0)
                {
                    freeServers = CustomerServer.servers.Where(m => !busyIds.Contains(m.UserId))?.ToList();
                }
                if (freeServers?.Count() > 0)
                {
                    CustomerServer.mapping.Add(new Customer(message.Conversation.Id, message.From.Id, message.From.Name, message.Recipient.Name, message.Recipient.Id, message.ServiceUrl), new CustomerServer(freeServers[0].ConversationId, freeServers[0].UserId, freeServers[0].Name, freeServers[0].BotId, freeServers[0].BotId, freeServers[0].ServiceUrl));
                    await context.PostAsync($"{freeServers[0].Name} is connecting you");

                }
                else
                {
                    await context.PostAsync($"all customerServers are busy ,pls wait");
                }
                context.Wait(MessageReceivedAsync);
                return;
            }
            //chat with cleverbot
            if (!InnerData.dic.ContainsKey(message.Text))
            {
                await context.Forward(new LuisDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else
            {
                await context.Forward(new EchoDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}