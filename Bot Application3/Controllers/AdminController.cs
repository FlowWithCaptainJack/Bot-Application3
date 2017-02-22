using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bot.model;
using BotApplicationDemo.Utilities;
using Microsoft.Bot.Connector;

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