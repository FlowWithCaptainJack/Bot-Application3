using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.model;
using Bot_Application3.Utilities;
using BotApplicationDemo.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    public class CustomerDialog : BaseDialog
    {
        public override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;
            using (var db = new BotdbUtil())
            {
                var user = db.Customer.Find(message.From.Id);
                try
                {
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
                            await context.PostAsync($"{customerServer.Name} is connecting you");
                            customerServer.Customer = user;
                            db.SaveChanges();
                        }
                        else
                        {
                            await context.PostAsync($"all customerServers are busy ,pls wait");
                        }
                        context.Wait(MessageReceivedAsync);
                        return;
                    }
                }
                catch (Exception ex)
                {

                    throw;
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