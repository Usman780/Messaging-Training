
using System.Web.Mvc;
using WebMarkupMin.AspNet4.Mvc;

namespace TrainingTracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
            filters.Add(new CompressContentAttribute());
            filters.Add(new MinifyHtmlAttribute());
            filters.Add(new MinifyXmlAttribute());
        }
    }
}
