using System.Linq;
using System.Web.Mvc;
using BowTie.View.Models;
using BowTie.BLL.Interfaces;

namespace BowTie.View.Controllers
{
    public class ClassificationsController : Controller
    {
        private readonly IEventTypeService _eventTypeService;
        private readonly IStatsService _statsService;
        private readonly IRegionService _regionService;

        public ClassificationsController(IEventTypeService eventTypeService, IStatsService statsService, IRegionService regionService)
        {
            _eventTypeService = eventTypeService;
            _statsService = statsService;
            _regionService = regionService;
        }

        public ActionResult Types()
        {
            var eventTypes = _eventTypeService.GetAllEventTypes()
                .Select(e => new EventTypeViewModel()
                {
                    Code = e.Code,
                    Name = e.Name,
                    Diagrams = e.TotalEventsCount,
                    ParentId = e.ParentCode
                })
                .ToList();
            return View(eventTypes);
        }

        public ActionResult Regions(int? startYear, int? endYear)
        {
            var all = _regionService.GetRegions(startYear, endYear)
                .Select(r => new RegionViewModel()
                {
                    Id = r.Id,
                    Name = r.RegionName,
                    Diagrams = r.EventsCount,
                    Stats = _statsService.GetStats(r.Id, startYear, endYear).ToList()
                }).OrderByDescending(r => r.Diagrams).ToList();
            ViewBag.StartYear = startYear;
            ViewBag.EndYear = endYear;
            return View(all);
        }
    }
}