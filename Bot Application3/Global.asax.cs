using System.Web.Http;
using System.Web.Routing;
using Bot_Application3.Utilities;

namespace Bot_Application3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_End()
        {
            using (var db = new BotdbUtil())
            {
                db.Database.ExecuteSqlCommand("delete from [dbo].[Admins];delete from [dbo].[CustomerServers];delete from [dbo].[Customers] ");
            }
        }
    }
}
