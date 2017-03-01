using System.Threading.Tasks;
using System.Web.Mvc;
using Bot_Application3.model;

namespace Bot.Controllers
{
    public class HomeController : Controller
    {
        // GET: Default
        public async Task<ActionResult> Index()
        {
            ViewData.Model = (await WebChatToken.GetToken(Session.SessionID)).Token;
            return View();
        }
    }
}