using System.Web;
using System.Web.Mvc;

namespace Buoi_4_Bai_BigSchool__
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
