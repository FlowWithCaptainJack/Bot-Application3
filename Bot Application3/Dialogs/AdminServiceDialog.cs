using System;
using System.Threading.Tasks;
using Bot.Utilities;
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
            using (var db = new BotdbUtil())
            {
                var admin = db.Admin.Find(message.From.Id);
                if (admin != null)
                {
                    //send to customer
                    db.CustomerMessage.Add(new model.CustomerMessage { CustomerId = admin.Customer.UserId, FromId = admin.Customer.BotId, Text = $"【{message.From.Name}】:{message.Text}", timestamp = DateTime.UtcNow.Ticks });
                    db.SaveChanges();
                    await SendActivity(admin.Customer, $"【{message.From.Name}】:{message.Text}");
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
