using System.Threading.Tasks;
using Bot_Application3.model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application3.Dialogs
{
    public abstract class BaseDialog : IDialog<object>
    {
        public abstract Task StartAsync(IDialogContext context);

        protected async Task ResumeAfterDialog(IDialogContext context, IAwaitable<object> result)
        {
            await result;
            context.Wait(MessageReceivedAsync);
        }
        public abstract Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument);
        protected async Task UpdateConversationData(IDialogContext context, Activity activity)
        {

            var botstate = activity.GetStateClient().BotState;
            var data = await botstate.GetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
            string value = data.GetProperty<string>("menuId");
            if (activity.Text == "0")
            {
                data.SetProperty("menuId", value.Remove(value.Length - 1));
                await botstate.SetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id, data);
                return;
            }
            if (InnerData.dic.ContainsKey($"{value}{activity.Text}"))
            {
                data.SetProperty("menuId", $"{value}{activity.Text}");
                await botstate.SetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id, data);
            }
        }

        protected async Task Forward(IDialogContext context, Activity activity, string step)
        {
            var botstate = activity.GetStateClient().BotState;
            var data = await botstate.GetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
            string value = data.GetProperty<string>("menuId");
            data.SetProperty("menuId", value + step);
            await botstate.SetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id, data);
        }
        protected async Task Back(IDialogContext context, Activity activity)
        {
            var botstate = activity.GetStateClient().BotState;
            var data = await botstate.GetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
            string value = data.GetProperty<string>("menuId");
            data.SetProperty("menuId", value.Remove(value.Length - 1));
            await botstate.SetPrivateConversationDataAsync(activity.ChannelId, activity.Conversation.Id, activity.From.Id, data);
        }
    }
}
