using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Bot_Application3.Utilities
{
    public class JsonNetResult : JsonResult
    {

        public JsonNetResult(object data)
        {
            Data = data;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss fff" }));
            }
        }


    }
}
