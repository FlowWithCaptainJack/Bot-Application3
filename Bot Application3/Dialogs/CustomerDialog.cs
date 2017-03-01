using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.model;
using Bot.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    public class CustomerDialog : BaseDialog
    {
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Microsoft.Bot.Connector.Activity;
            using (var db = new BotdbUtil())
            {
                //add activity for customer
                db.CustomerMessage.Add(new CustomerMessage { CustomerId = message.From.Id, FromId = message.From.Id, Text = message.Text, timestamp = message.Timestamp.Value.Ticks });
                db.SaveChanges();
                var user = db.Customer.Find(message.From.Id);
                // check admin server
                if (db.Admin.Count(m => m.Customer != null && m.Customer.UserId == user.UserId) < 1)
                {
                    string conversationId = await BotUtil.StartConversation();
                    db.Admin.Add(new Admin(conversationId, "admin" + user.UserId, "admin", "", "", "") { Customer = user });
                    db.SaveChanges();
                    await BotUtil.SendActivity(conversationId, "admin" + user.UserId, "admin", "regist admin server");
                }
                // chat with admin?
                if (!user.BotEnabled)
                {
                    var admin = db.Admin.FirstOrDefault(m => m.Customer.UserId == user.UserId);
                    await SendActivity(admin, $"【{message.From.Name}】:{message.Text}");
                    context.Wait(MessageReceivedAsync);
                    return;
                }
                //chat with customerService?
                if (db.CustomerServer.Count(m => m.Customer.UserId == user.UserId) > 0)
                {
                    //send to customerServer(customerRole)
                    var customerServer = db.CustomerServer.FirstOrDefault(m => m.Customer.UserId == user.UserId);
                    await SendActivity(customerServer, $"【{message.From.Name}】:{message.Text}");
                    context.Wait(MessageReceivedAsync);
                    return;
                }
                //connect to customerService?
                if (message.Text == "9")
                {
                    var customerServer = db.CustomerServer.Where(m => m.Customer == null).FirstOrDefault();
                    if (customerServer != null)
                    {
                        db.CustomerMessage.Add(new CustomerMessage { CustomerId = message.From.Id, FromId = message.Recipient.Id, Text = $"{customerServer.Name} is connecting you", timestamp = DateTime.UtcNow.Ticks });
                        await context.PostAsync($"{customerServer.Name} is connecting you");
                        customerServer.Customer = user;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.CustomerMessage.Add(new CustomerMessage { CustomerId = message.From.Id, FromId = message.Recipient.Id, Text = $"all customerServers are busy ,pls wait", timestamp = DateTime.UtcNow.Ticks });
                        await context.PostAsync($"all customerServers are busy ,pls wait");
                    }
                    context.Wait(MessageReceivedAsync);
                    return;
                }
            }

            //chat with cleverbot?
            if (!InnerData.dic.ContainsKey(message.Text))
            {
                await context.Forward(new CleverBotDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else
            {
                await context.Forward(new EchoDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
        }
    }
}