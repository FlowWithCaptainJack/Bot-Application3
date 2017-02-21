using System;
using System.Linq;
using System.Threading.Tasks;
using Bot.model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    class AdminServiceDialog : BaseDialog
    {
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;
            if (Admin.mapping.Count(m => m.Value.ConversationId == message.Conversation.Id) > 0)
            {
                var map = Admin.mapping.FirstOrDefault(m => m.Value.UserId == message.From.Id);
                var user = map.Key;
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
