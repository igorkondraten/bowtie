using System.Web.Mvc;
using BowTie.BLL.Interfaces;

namespace BowTie.View.Controllers
{
    public class SavedDiagramsController : Controller
    {
        private readonly ISavedDiagramService _savedDiagramService;

        public SavedDiagramsController(ISavedDiagramService savedDiagramService)
        {
            _savedDiagramService = savedDiagramService;
        }

        [Authorize(Roles = "Адміністратор, Експерт")]
        [HttpPost]
        public ActionResult Verify(int savedDiagramId, bool isVerified)
        {
            _savedDiagramService.SetVerification(isVerified, savedDiagramId);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}