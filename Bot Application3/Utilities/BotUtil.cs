using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bot.model;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Bot.Utilities
{
    class BotUtil
    {
        public static async Task<string> StartConversation()
        {
            using (var clientBot = new HttpClient())
            {
                clientBot.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "pcz91rOW0-c.cwA.xl4.Sd7Gr_GYmyocRz4iwpXrKTMqAkOzh0khv-3UA1gRles");
                var response = await clientBot.PostAsync($"https://directline.botframework.com/v3/directline/conversations", new StringContent(""));
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(result).conversationId.ToString();
                }
            }
            return "";
        }

        public static async Task SendActivityFromBot(string conversationId, ChannelAccount user, ChannelAccount bot, string message, string serviceUrl)
        {
            using (var connector = new ConnectorClient(new Uri(serviceUrl)))
            {
                var activity = Microsoft.Bot.Connector.Activity.CreateMessageActivity() as Microsoft.Bot.Connector.Activity;
                activity.Conversation = new ConversationAccount(id: conversationId);
                activity.From = bot;
                activity.Recipient = user;
                activity.Text = message;
                activity.Locale = "en-Us";
                //activity.Text = JsonConvert.SerializeObject(activity);
                await connector.Conversations.SendToConversationAsync(activity);
            }
        }

        public static async Task<BotActivity> GetActivites(string conversationId, string watermark = "")
        {
            using (var clientBot = new HttpClient())
            {
                clientBot.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "pcz91rOW0-c.cwA.xl4.Sd7Gr_GYmyocRz4iwpXrKTMqAkOzh0khv-3UA1gRles");
                var response = await clientBot.GetAsync($"https://directline.botframework.com/v3/directline/conversations/{  conversationId }/activities?watermark={watermark ?? ""} ");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BotActivity>(content);
                }
            }
            return null;
        }

        public static async Task<string> SendActivity(string conversationId, string userId, string userName, string message)
        {
            using (var clientBot = new HttpClient())
            {
                clientBot.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "pcz91rOW0-c.cwA.xl4.Sd7Gr_GYmyocRz4iwpXrKTMqAkOzh0khv-3UA1gRles");
                var content = JsonConvert.SerializeObject(new
                {
                    type = "message",
                    from = new { id = userId, name = userName },
                    channelId = "directline",
                    timestamp = DateTime.Now,
                    text = message
                }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var response = await clientBot.PostAsync($"https://directline.botframework.com/v3/directline/conversations/{  conversationId }/activities", new StringContent(content, Encoding.UTF8, "application/json"));
                string temp = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).id.ToString();
                }
                return "";
            }
        }

        public static async Task<ChannelAccount[]> GetConversationMembers(string conversationId, string serviceUrl)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(serviceUrl), "39621911-458e-4c0d-a3c4-418dedce04de", "95mnH7SmD5ADf6u5EMU8Gug");

            return await connector.Conversations.GetConversationMembersAsync(conversationId);
        }
    }
}
