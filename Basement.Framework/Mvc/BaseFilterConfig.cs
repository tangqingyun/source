using System.Web;
using System.Web.Mvc;

namespace Basement.Framework.Mvc
{
    public class BaseFilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}