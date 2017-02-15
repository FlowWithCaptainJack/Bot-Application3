using System.Threading.Tasks;
using Bot_Application3.model;
using BotApplicationDemo.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application3.Dialogs
{
    public abstract class BaseDialog : IDialog<object>
    {
        public virtual async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public abstract Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument);

        protected async Task ResumeAfterDialog(IDialogContext context, IAwaitable<object> result)
        {
            await result;
            context.Wait(MessageReceivedAsync);
        }
        protected async Task SendActivity(Customer user, string message)
        {
            string serviceUrl = user.ServiceUrl;
            var userAccount = new ChannelAccount(id: user.UserId);
            var botAccount = new ChannelAccount(id: user.BotId, name: user.BotName);
            var conversationId = user.ConversationId;
            await BotUtil.SendActivityFromBot(conversationId, userAccount, botAccount, message, serviceUrl);
        }
        protected async Task SendActivity(CustomerServer user, string message)
        {
            string serviceUrl = user.ServiceUrl;
            var userAccount = new ChannelAccount(id: user.UserId, name: user.Name);
            var botAccount = new ChannelAccount(id: user.BotId, name: user.BotName);
            var conversationId = user.ConversationId;
            await BotUtil.SendActivityFromBot(conversationId, userAccount, botAccount, message, serviceUrl);
        }
    }
}
