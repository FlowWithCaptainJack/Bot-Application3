using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using BotApplicationDemo.model;
using BotApplicationDemo.Utilities;
using Tencent;

namespace Bot_ApplicationDemo.ApiControllers
{

    public class WechatController : ApiController
    {
        static Queue<string> wechatMessage = new Queue<string>();
        static Dictionary<string, Conversation> userConversation = new Dictionary<string, Conversation>();
        private static WXBizMsgCrypt cry = new WXBizMsgCrypt("8GoRCJfhVRsqPKFAwzO227b4wR4oifI", "a79i3rLU7drrzqvK8vC3CmC7DhaS6tl3IhYELFN0WxQ", "wx4939e0f62caad7dc");

        static WechatController()
        {
            ThreadPool.QueueUserWorkItem(RecieveActivities);
        }
        // GET: Wechat
        public HttpResponseMessage Get(string echostr, string msg_signature, string timestamp, string nonce)
        {
            string msg = "";
            HttpStatusCode code = HttpStatusCode.BadRequest;
            int flag = cry.VerifyURL(msg_signature, timestamp, nonce, echostr, ref msg);
            if (flag == 0)
            {
                code = HttpStatusCode.OK;
            }
            var response = new HttpResponseMessage(code);
            response.Content = new StringContent(msg);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            return response;
        }
        public async Task Post(string msg_signature, string timestamp, string nonce)
        {
            string inMsgStr = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            string decryptedMsg = "";
            int flag = cry.DecryptMsg(msg_signature, timestamp, nonce, inMsgStr, ref decryptedMsg);
            if (flag != 0)
            {
                return;
            }

            var weMsg = new XmlSerializer(typeof(WeChatMessage)).Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(decryptedMsg))) as WeChatMessage;
            if (wechatMessage.Count > 1000)
            {
                wechatMessage.Dequeue();
            }
            if (wechatMessage.Contains(weMsg.UserOpenId + weMsg.MessageId))
            {
                return;
            }
            wechatMessage.Enqueue(weMsg.UserOpenId + weMsg.MessageId);
            string conversationId = "";

            if (weMsg.MessageType == MessageType.Event && weMsg.EventType == EventType.Subscribe)
            {
                if (userConversation.ContainsKey(weMsg.UserOpenId))
                {
                    conversationId = userConversation.FirstOrDefault(m => m.Key == weMsg.UserOpenId).Value.id;
                }
                else
                {
                    conversationId = await BotUtil.StartConversation();
                    userConversation.Add(weMsg.UserOpenId, new Conversation { id = conversationId, watermark = null });
                }
                return;
            }

            if (weMsg.MessageType == MessageType.Text)
            {
                if (userConversation.ContainsKey(weMsg.UserOpenId))
                {
                    conversationId = userConversation.FirstOrDefault(m => m.Key == weMsg.UserOpenId).Value.id;
                }
                else
                {
                    conversationId = await BotUtil.StartConversation();
                    userConversation.Add(weMsg.UserOpenId, new Conversation { id = conversationId, watermark = null });
                }
                //SendActivity 
                string replyid = await BotUtil.SendActivity(conversationId, weMsg.UserOpenId, await Wechat.GetUserName(weMsg.UserOpenId), weMsg.Content);
            }
        }

        private static async void RecieveActivities(object o)
        {
            while (true)
            {
                var temp = userConversation.Where(m => true);
                foreach (var item in temp)
                {
                    var botactivity = await BotUtil.GetActivites(item.Value.id, item.Value.watermark);
                    if (botactivity?.activities?.Count(m => m.from.id != item.Key) > 0)
                    {
                        item.Value.watermark = botactivity.watermark;
                        foreach (var activity in botactivity.activities.Where(m => m.from.id != item.Key))
                        {
                            // send message to wechat
                            Wechat.PostMessageToUser(item.Key, activity.text);
                        }
                    }
                }
                if (temp.Count() < 1)
                {
                    await Task.Delay(1000);
                }
            }

        }

        class Conversation
        {
            public string id { get; set; }
            public string watermark { get; set; }
        }
    }

}