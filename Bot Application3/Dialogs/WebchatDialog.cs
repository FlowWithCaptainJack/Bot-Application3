using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Bot_Application3.Dialogs
{
    [Serializable]
    public class WebchatDialog : BaseDialog
    {
        public override async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync(JsonConvert.SerializeObject(message));
            context.Wait(MessageReceivedAsync);
        }
    }
}
