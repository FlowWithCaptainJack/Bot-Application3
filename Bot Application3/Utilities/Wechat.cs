using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BotApplicationDemo.Utilities
{
    public class Wechat
    {
        private static async Task<string> GetAccessToken(string corpid = "wx4939e0f62caad7dc", string corpsecret = "7dRwScpmbpaKBzf8RxjuSgSJgnNjykpkWk68ivTO5hGe3VpywnqKUQwO2VL9AUVe")
        {
            using (var clientWechat = new HttpClient())
            {
                var response = await clientWechat.GetAsync($"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={ corpid }&corpsecret={ corpsecret}");
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).access_token.ToString();
                }
            }
            return "";
        }

        public static async void PostMessageToUser(string userId, string text)
        {
            using (var clientWechat = new HttpClient())
            {
                var message = JsonConvert.SerializeObject(new
                {
                    touser = userId,
                    msgtype = "text",
                    agentid = 12,
                    text = new { content = text }
                }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                await clientWechat.PostAsync($"https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={ await GetAccessToken()}", new StringContent(message, Encoding.UTF8, "application/json"));
            }

        }

        public static async Task<string> GetUserName(string userId)
        {
            using (var clientWechat = new HttpClient())
            {
                var response = await clientWechat.GetAsync($"https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={await GetAccessToken()}&userid={userId}");
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).name.ToString();
                }
            }
            return "";
        }
    }
}