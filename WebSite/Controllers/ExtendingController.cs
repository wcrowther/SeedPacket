using System.Web.Mvc;

namespace Website.Controllers
{
    public class ExtendingController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}