using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace Bot_Application3.Dialogs
{
    [Serializable]
    [LuisModel("defbab3b-2a68-42fe-b123-61fd28c59dd3", "60d8f3593d1e4a5b90832dd9af3fae2e")]
    public class LuisDialog : LuisDialog<object>
    {
        Activity message;
        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            message = await item as Activity;
            await base.MessageReceived(context, item);
        }
        [LuisIntent("HostProblem")]
        public async Task Host(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"lisi will contact you later");
            context.Wait(MessageReceived);
        }

        [LuisIntent("LapTopProblem")]
        public async Task LapTop(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"wanger will contact you later");
            context.Wait(MessageReceived);
        }

        [LuisIntent("MonitorProblem")]
        public async Task Monitor(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"zhangsan will contact you later");
            context.Wait(MessageReceived);
        }
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string messages = $"Sorry I did not understand what you said.";
            await context.PostAsync(messages);
            context.Wait(MessageReceived);
        }
    }
}
