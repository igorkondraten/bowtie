using System;
using System.Web.Mvc;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.View.Models;

namespace BowTie.View.Controllers
{
    public class DiagramUpdatesController : Controller
    {
        private readonly IDiagramUpdateService _diagramUpdateService;
        private readonly IUserService _userService;

        public DiagramUpdatesController(IDiagramUpdateService diagramUpdateService, IUserService userService)
        {
            _diagramUpdateService = diagramUpdateService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost]
        public ActionResult Save(DiagramUpdateViewModel data)
        {
            var user = _userService.GetUser(User.Identity.Name);
            if (user == null)
                return View("Error");
            var diagramUpdate = new DiagramUpdateDTO()
            {
                Date = DateTime.Now,
                JsonDiagram = data.JsonDiagram,
                SavedDiagramId = data.SavedDiagramId,
                Updates = data.Updates,
                UserId = user.Id
            };
            var createdDiagramUpdate = _diagramUpdateService.CreateDiagramUpdate(diagramUpdate);
            return RedirectToAction("View", "Events", new { id = createdDiagramUpdate.SavedDiagram.EventId });
        }

        [Authorize(Roles = "Адміністратор")]
        [HttpPost]
        public ActionResult DeleteUpdate(int id)
        {
            _diagramUpdateService.DeleteDiagramUpdate(id);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}