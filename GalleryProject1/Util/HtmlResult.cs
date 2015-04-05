using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryProject.Util
{
    public class HtmlResult : ActionResult
    {
        private string htmlCode;

        public HtmlResult(string html)
        {
            htmlCode = html;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string fullHtmlCode = htmlCode;
            context.HttpContext.Response.Write(fullHtmlCode);
        }
    }
}