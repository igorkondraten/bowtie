using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BowTie.BLL.DTO;
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
            ViewBag.StartYear = startYear;
            ViewBag.EndYear = endYear;
            var all = _regionService.GetRegions(startYear, endYear)
                .Select(r => new
                {
                    Region = r,
                    Stats = _statsService.GetStats(r.Id, startYear, endYear).Select(x => new Stats()
                    { Count = x.Count, EventTypeName = x.EventTypeName, Region = r.RegionName }).ToList()
                }).OrderByDescending(r => r.Region.EventsCount).ToList();
            var allStats = new List<Stats>();
            all.ForEach(x => allStats.AddRange(x.Stats));
            var model = new RegionViewModel();
            model.Regions = all.Select(x => x.Region).ToList();
            model.EventStats = allStats.GroupBy(x => x.EventTypeName).Select(x =>
                new EventStats()
                {
                    EventType = x.Key,
                    Stats = x.Select(z => new Stats()
                    {
                        Count = z.Count,
                        Region = z.Region
                    }).ToList(),
                }).ToList();
            return View(model);
        }
    }
}