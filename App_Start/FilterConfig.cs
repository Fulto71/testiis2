using System.Web;
using System.Web.Mvc;

namespace ZenithWebServeur.WCF
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
