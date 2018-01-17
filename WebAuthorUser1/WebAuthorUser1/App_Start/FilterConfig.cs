using System.Web;
using System.Web.Mvc;
using WebAuthorUser1.ErrorHandler;

namespace WebAuthorUser1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
            // filters.Add(new HandleErrorAttribute());
        }
    }
}
