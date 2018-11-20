using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BowTie.View.Models;
using BowTie.BLL.DTO;
using BowTie.BLL.Services;
using BowTie.BLL.Interfaces;

namespace BowTie.View.Controllers
{
    public class SavesController : ApiController
    {
        IBowTieService service = new BowTieService();

        [HttpGet]
        [ResponseType(typeof(SaveData))]
        public IHttpActionResult GetSave(int id)
        {
            SaveDTO save;
            try
            {
                save = service.GetSave(id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            SaveData @json = new SaveData();
            if (save != null)
            {
                @json = new SaveData() { Json = save.JsonDiagram, Name = save.Diagram.EventName, DiagramId = save.DiagramId };
            }
            return Ok(@json);
        }
    }
}
