using System;
using System.Threading.Tasks;
using Bot.model;
using Bot.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
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
            var message = await argument as Microsoft.Bot.Connector.Activity;
            string result = InnerData.dic.ContainsKey(message.Text) ? InnerData.dic[message.Text] : message.Text;
            using (var db = new BotdbUtil())
            {
                db.CustomerMessage.Add(new CustomerMessage { CustomerId = message.From.Id, FromId = message.Recipient.Id, Text = result, timestamp = DateTime.UtcNow.Ticks });
                db.SaveChanges();
            }
            await context.PostAsync(result);
            context.Done(1);
        }
    }
}
