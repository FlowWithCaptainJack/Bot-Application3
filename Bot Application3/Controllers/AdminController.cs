using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bot.model;
using BotApplicationDemo.Utilities;
using Microsoft.Bot.Connector;
using Microsoft.Cognitive.LUIS;

namespace Bot_Application3.Controllers
{
    public class AdminController : Controller
    {

        // GET: Default
        public ActionResult Index()
        {
            ViewData.Add("mapping", CustomerServer.mapping);
            return View();
        }


        //Get intelligence

        public async Task<ActionResult> GetIntelligence(string question)
        {
            LuisClient luis = new LuisClient("c9e8acee-c856-450e-973e-176a5b90aa45", "60d8f3593d1e4a5b90832dd9af3fae2e");
            var result = (await luis.Predict(question)).Intents.Where(m => m.Score > 0.2)?.OrderByDescending(m => m.Score).Take(3);
            if (result?.Count() > 0)
            {
                var answer = InnerData.questionAnswer.Where(m => result.Count(n => n.Name == m.Key) > 0)?.Select(m => m.Value);
                if (answer?.Count() > 0)
                {
                    return Json(new { count = answer.Count(), content = answer }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// send message to customer
        /// </summary>
        /// <param name="content"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Index(string content, string customerId)
        {
            Customer.Customers.FirstOrDefault(m => m.UserId == customerId).BotEnabled = false;
            if (Admin.mapping.Count(m => m.Key.UserId == customerId) > 0)
            {
                var user = Admin.mapping.FirstOrDefault(m => m.Key.UserId == customerId).Key;
                string serviceUrl = user.ServiceUrl;
                var userAccount = new ChannelAccount(id: user.UserId, name: user.Name);
                var botAccount = new ChannelAccount(id: user.BotId, name: user.BotName);
                var conversationId = user.ConversationId;
                await BotUtil.SendActivityFromBot(conversationId, userAccount, botAccount, content, serviceUrl);
                return Json(new { status = true });
            }
            return Json(new { status = false });
        }
        public async Task<ActionResult> GetMessages(string customerId, string watermark = "")
        {
            var user = Customer.Customers.FirstOrDefault(m => m.UserId == customerId);
            var result = await BotUtil.GetActivites(user.ConversationId, watermark);
            result.activities = result.activities?.Where(m => !string.IsNullOrWhiteSpace(m.from?.id))?.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}