using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Bot.Dialogs
{
    [Serializable]
    [LuisModel("defbab3b-2a68-42fe-b123-61fd28c59dd3", "60d8f3593d1e4a5b90832dd9af3fae2e")]
    public class CleverBotDialog : LuisDialog<object>
    {
        [LuisIntent("HostProblem")]
        public async Task Host(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"lisi will contact you later");
            context.Done(1);
        }

        [LuisIntent("LapTopProblem")]
        public async Task LapTop(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"wanger will contact you later");
            context.Done(1);
        }

        [LuisIntent("MonitorProblem")]
        public async Task Monitor(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"zhangsan will contact you later");
            context.Done(1);
        }
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string messages = $"Sorry I did not understand what you said， if you need chatServer,pls enter \"9\"";
            await context.PostAsync(messages);
            context.Done(1);
        }
    }
}
