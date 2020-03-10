using System.Web.Mvc;

namespace Website.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        protected override void HandleUnknownAction(string actionName)
        {
            View(actionName).ExecuteResult(this.ControllerContext);
        }
    }
}
