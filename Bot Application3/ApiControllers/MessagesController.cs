using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bot.Dialogs;
using Bot.model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Bot_ApplicationDemo.ApiControllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        static Queue<string> messages = new Queue<string>();
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                //filter repeated messages
                if (messages.Count > 1000)
                {
                    messages.Dequeue();
                }
                if (messages.Contains(activity.From.Id + activity.Id))
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                }
                messages.Enqueue(activity.From.Id + activity.Id);

                //register for a customerService?
                if (activity.Text == "i am a superman")
                {
                    if (CustomerServer.servers.Count(m => m.UserId == activity.From.Id) > 0)
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                    }
                    if (Customer.Customers.Count(m => m.UserId == activity.From.Id) > 0)
                    {
                        foreach (var item in Customer.Customers.Where(m => m.UserId == activity.From.Id))
                        {
                            Customer.Customers.Remove(item);
                        }
                    }
                    CustomerServer.servers.Add(new CustomerServer(activity.Conversation.Id, activity.From.Id, activity.From.Name, activity.Recipient.Name, activity.Recipient.Id, activity.ServiceUrl));
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    var result = activity.CreateReply($"successfully that  you are a superman!!! details:{JsonConvert.SerializeObject(activity)}");
                    await connector.Conversations.ReplyToActivityAsync(result);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                }
                //register for a admin server?
                if (activity.Text == "regist admin server")
                {
                    if (Admin.Admins.Count(m => m.UserId == activity.From.Id) > 0)
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                    }
                    Admin admin = new Admin(activity.Conversation.Id, activity.From.Id, activity.From.Name, activity.Recipient.Name, activity.Recipient.Id, activity.ServiceUrl);
                    Admin.Admins.Add(admin);
                    var customerLeft = Customer.Customers.FirstOrDefault(m => Admin.mapping.Count(n => n.Key.UserId == m.UserId) < 1);
                    Admin.mapping.Add(new Customer(customerLeft.ConversationId, customerLeft.UserId, customerLeft.Name, customerLeft.BotName, customerLeft.BotId, customerLeft.ServiceUrl), new Admin(admin.ConversationId, admin.UserId, admin.Name, admin.BotName, admin.BotId, admin.ServiceUrl));
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                }

                //register for a customer(default)
                if (CustomerServer.servers.Count(m => m.UserId == activity.From.Id) < 1 && Customer.Customers.Count(m => m.UserId == activity.From.Id) < 1)
                {
                    Customer.Customers.Add(new Customer(activity.Conversation.Id, activity.From.Id, activity.From.Name, activity.Recipient.Name, activity.Recipient.Id, activity.ServiceUrl));
                }

                //switch to 3 workflows(customer/customerService/adminservice)
                if (Customer.Customers.Count(m => m.UserId == activity.From.Id) > 0)
                {
                    await Conversation.SendAsync(activity, () => new CustomerDialog());
                }
                if (CustomerServer.servers.Count(m => m.UserId == activity.From.Id) > 0)
                {
                    await Conversation.SendAsync(activity, () => new CustomerServiceDialog());
                }
                if (Admin.Admins.Count(m => m.UserId == activity.From.Id) > 0)
                {
                    await Conversation.SendAsync(activity, () => new AdminServiceDialog());
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
                if (message.MembersAdded?.Count() > 0 && message.MembersAdded.Count(m => !m.Name.ToLower().Contains("bot")) > 0)
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