using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowTie.View.Models;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.BLL.Services;

namespace BowTie.View.Controllers
{
    public class ClassificationsController : Controller
    {
        IBowTieService service = new BowTieService();

        // Повертає сторінку з класифікацією надзвичайних ситуацій
        public ActionResult Types()
        {
            IEnumerable<EventViewModel> eventTypes;
            try
            {
                eventTypes = service.GetEventTypes().Select(e => new EventViewModel() { Code = e.Code, Name = e.Name, Diagrams = e.Diagrams, ParentId = e.ParentCode }).ToList();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            return View(eventTypes);
        }

        // Повертає статистику по регіонам та по рокам починаючи з startYear, закінчуючи з endYear.
        // Без параметрів повертає статистику за усі роки
        public ActionResult Regions(int? startYear, int? endYear)
        {
            List<RegionViewModel> all;
            try
            {
                all = service.GetRegions(startYear, endYear).Select(r => new RegionViewModel() { Id = r.Id, Name = r.RegionName, Diagrams = r.Diagrams, Stats = service.GetStats(r.Id, startYear, endYear).Select(s => new TypeStats() { Name = s.GetType().GetProperty("Name").GetValue(s, null), Count = s.GetType().GetProperty("Count").GetValue(s, null) }).ToList() }).OrderByDescending(r => r.Diagrams).ToList();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }

            // Запис років у ViewBag для відображення на сторінці
            ViewBag.StartYear = startYear;
            ViewBag.EndYear = endYear;

            return View(all);
        }
    }
}