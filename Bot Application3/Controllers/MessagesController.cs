using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bot_Application3.Dialogs;
using Bot_Application3.model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                switch (activity.ChannelId.ToLower())
                {
                    case "directline": await Conversation.SendAsync(activity, () => new WechatDialog()); break;
                    case "skype": await Conversation.SendAsync(activity, () => new SkypeDialog()); break;
                    case "webchat": await Conversation.SendAsync(activity, () => new WebchatDialog()); break;
                    default:
                        break;
                }
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded?.Count(m => !m.Name.ToLower().Contains("bot")) > 0)
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    var result = message.CreateReply(InnerData.dic["0"]);
                    await connector.Conversations.ReplyToActivityAsync(result);
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }


}