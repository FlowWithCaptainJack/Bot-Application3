using System;
using System.Linq;
using System.Threading.Tasks;
using Bot.model;
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
            if (CustomerServer.mapping.Count(m => m.Value.ConversationId == message.Conversation.Id) > 0)
            {
                var map = CustomerServer.mapping.FirstOrDefault(m => m.Value.UserId == message.From.Id);
                var user = map.Key;
                // stop chat with current customer?
                if (message.Text.ToLower() == "#exit#")
                {
                    CustomerServer.mapping.Remove(map.Key);
                    await SendActivity(map.Key, $"已经断开与客服【{map.Value.Name}】的连接");
                    await SendActivity(map.Value, $"已经断开与客户【{map.Key.Name}】的连接");
                    context.Wait(MessageReceivedAsync);
                    return;
                }
                //send to customer
                await SendActivity(map.Key, $"【{message.From.Name}】:{message.Text}");
            }
            else
            {
                await context.PostAsync($"no user need help");
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}
