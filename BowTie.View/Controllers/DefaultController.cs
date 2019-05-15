using System.Web.Mvc;

namespace BowTie.View.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}