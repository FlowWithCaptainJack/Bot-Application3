using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bot.model;
using Bot.Utilities;
using Bot_Application3.Utilities;
using Microsoft.Cognitive.LUIS;

namespace Bot.Controllers
{
    public class AdminController : Controller
    {

        // GET: Default
        public ActionResult Index()
        {
            using (var db = new BotdbUtil())
            {
                ViewData.Add("customers", db.Customer?.ToList());
            }
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
            using (var db = new BotdbUtil())
            {
                db.Customer.Find(customerId).BotEnabled = false;
                db.SaveChanges();
                if (db.Admin.Count(m => m.Customer.UserId == customerId) > 0)
                {
                    var admin = db.Admin.FirstOrDefault(m => m.Customer.UserId == customerId);
                    var conversationId = admin.ConversationId;
                    await BotUtil.SendActivity(conversationId, admin.UserId, admin.Name, content);
                    return Json(new { status = true });
                }
            }
            return Json(new { status = false });
        }
        public async Task<ActionResult> GetMessages(string customerId, long timestamp = 0)
        {
            using (var db = new BotdbUtil())
            {
                var user = db.Customer.Find(customerId);
                var messages = db.CustomerMessage.Where(m => m.CustomerId == user.UserId && m.timestamp > timestamp);
                if (messages?.Count() > 0)
                {
                    return new JsonNetResult(new { count = messages.Count(), content = messages.ToList().OrderBy(m => m.timestamp), timestamp = messages.ToList().OrderByDescending(m => m.timestamp).FirstOrDefault().timestamp });
                }
                return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> ChangeAutoStatus(string customerId, bool status)
        {
            using (var db = new BotdbUtil())
            {
                db.Customer.Find(customerId).BotEnabled = status;
                await db.SaveChangesAsync();
            }
            return Json(new { status = status });
        }
    }
}