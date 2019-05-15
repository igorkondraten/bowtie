using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using BowTie.View.Models;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;

namespace BowTie.View.Controllers
{
    public class SavesController : ApiController
    {
        private readonly IDiagramUpdateService _diagramUpdateService;

        public SavesController(IDiagramUpdateService diagramUpdateService)
        {
            _diagramUpdateService = diagramUpdateService;
        }

        [HttpGet]
        [ResponseType(typeof(DiagramUpdateViewModel))]
        public IHttpActionResult GetUpdate(int id)
        {
            DiagramUpdateDTO save;
            try
            {
                save = _diagramUpdateService.GetDiagramUpdate(id);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            DiagramUpdateViewModel model = new DiagramUpdateViewModel();
            if (save != null)
            {
                model = new DiagramUpdateViewModel() { JsonDiagram = save.JsonDiagram, EventName = save.SavedDiagram.Event.EventName, SavedDiagramId = save.SavedDiagramId };
            }
            return Ok(model);
        }

        [HttpGet]
        [ResponseType(typeof(DiagramUpdateViewModel))]
        public IHttpActionResult GetLastUpdateId(int savedDiagramId)
        {
            List<DiagramUpdateDTO> saves;
            try
            {
                saves = _diagramUpdateService.GetUpdatesForDiagram(savedDiagramId).ToList();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            if (saves.Count == 0)
                return Ok(new { Id = 0 });
            return Ok(new { Id = saves.FirstOrDefault().Id });
        }
    }
}
