// From StackOverflow with modifications (TODO: insert link)

using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ezNReporting.Web.Controllers
{
    class JsonNetResult : JsonResult
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings();
        private static readonly JsonSerializer _serializer = JsonSerializer.Create(_settings);

        public JsonNetResult(object data)
            : base()
        {
            this.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
            this.Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrWhiteSpace(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            using (JsonTextWriter writer = new JsonTextWriter(response.Output))
            {
                writer.Formatting = Formatting.None;

                _serializer.Serialize(writer, this.Data);
            }
        }
    }
}