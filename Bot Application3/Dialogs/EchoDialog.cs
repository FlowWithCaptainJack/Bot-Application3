using System;
using System.Threading.Tasks;
using Bot_Application3.model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application3.Dialogs
{
    [Serializable]
    public class EchoDialog : BaseDialog
    {
        public override async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;
            await context.PostAsync(InnerData.dic.ContainsKey(message.Text) ? InnerData.dic[message.Text] : message.Text);
            //context.Wait(MessageReceivedAsync);
            context.Done(1);
        }
    }
}
