using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Bot.Utilities;
namespace Bot_Application3.model
{
    class WebChatToken
    {
        private Timer timer;
        private static List<WebChatToken> tokens = new List<WebChatToken>();
        public string SessionId { get; set; }

        public string Token { get; set; }

        private WebChatToken(string sessionId, string token)
        {
            SessionId = sessionId;
            Token = token;
            timer = new Timer(60 * 3);
            timer.Elapsed += Elapsed_Event;
            tokens.Add(this);
            timer.Start();
        }

        public static async Task<WebChatToken> GetToken(string sessionId)
        {
            var result = tokens.Where(m => m.SessionId == sessionId).FirstOrDefault();
            if (result == null)
            {
                return new WebChatToken(sessionId, (await BotUtil.GetWebChatToken()).Trim('"'));
            }
            return result;
        }

        private void Elapsed_Event(object sender, ElapsedEventArgs e)
        {
            timer.Elapsed -= Elapsed_Event;
            tokens.Remove(this);
        }
    }
}
