using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.View.Models;

namespace BowTie.View.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventTypeService _eventTypeService;
        private readonly ICityService _cityService;
        private readonly IRegionService _regionService;
        private readonly IDistrictService _districtService;

        public EventsController(IEventService eventService,
            ICityService cityService,
            IRegionService regionService,
            IDistrictService districtService,
            IEventTypeService eventTypeService)
        {
            _eventService = eventService;
            _regionService = regionService;
            _cityService = cityService;
            _districtService = districtService;
            _eventTypeService = eventTypeService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add(int? id)
        {
            AddEventViewModel model = new AddEventViewModel();
            var eventTypes = _eventTypeService.GetAllEventTypes();
            var regions = _regionService.GetAllRegions();
            var events = eventTypes.ToList().Select(e => new { Code = e.Code, Name = e.Code.ToString() + " " + e.Name });
            if (id.HasValue)
            {
                model.EventTypeCode = id.Value;
                List<SelectListItem> listEvents = new List<SelectListItem>();
                foreach (var item in eventTypes)
                {
                    listEvents.Add(new SelectListItem { Text = item.Code.ToString() + " " + item.Name, Value = item.Code.ToString(), Selected = (item.Code == id.Value) });
                }
                model.EventTypes = listEvents;
            }
            else
                model.EventTypes = new SelectList(events, "Code", "Name");
            model.Regions = new SelectList(regions.ToList(), "Id", "RegionName");
            model.Districts = new List<SelectListItem>();
            model.Cities = new List<SelectListItem>();
            model.Date = DateTime.Now;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(AddEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                EventDTO newEvent = new EventDTO()
                {
                    Id = new Guid(),
                    EventDate = model.Date,
                    EventName = model.Name,
                    EventTypeCode = model.EventTypeCode,
                    Info = model.Info,
                    Place = new PlaceDTO()
                    {
                        Address = model.Address,
                        CityId = model.CityId,
                        DistrictId = model.DistrictId,
                        RegionId = model.RegionId
                    }
                };
                var id = _eventService.CreateEvent(newEvent);
                return RedirectToAction("View", new { id = id });
            }
            var eventTypes = _eventTypeService.GetAllEventTypes();
            var regions = _regionService.GetAllRegions();
            model.EventTypes = new SelectList(eventTypes.ToList().Select(e => new { Code = e.Code, Name = e.Code.ToString() + " " + e.Name }), "Code", "Name");
            model.Regions = new SelectList(regions.ToList(), "Id", "RegionName");
            model.Cities = new List<SelectListItem>();
            model.Districts = new List<SelectListItem>();
            if (model.RegionId != 0)
            {
                model.Districts = new SelectList(_districtService.GetDistrictsForRegion(model.RegionId), "Id", "Name");
            }
            if (model.DistrictId.HasValue)
            {
                model.Cities = new SelectList(_cityService.GetCitiesForDistrict(model.DistrictId.Value).ToList(), "Id", "Name");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Region(int? id, int? startYear, int? endYear)
        {
            if (id == null)
            {
                return RedirectToAction("Regions", "Classifications");
            }
            var diagrams = _eventService.GetEventsByRegion(id.Value, startYear, endYear);
            var region = _regionService.GetRegion(id.Value);
            var model = diagrams.Select(d => new EventViewModel()
            {
                Id = d.Id,
                Event = d.EventTypeCode.ToString() + " " + d.EventType.Name,
                EventDate = d.EventDate,
                EventName = d.EventName,
                Info = d.Info,
                Region = d.Place.Region.RegionName,
                Address = d.Place.Address,
                City = (d.Place.CityId.HasValue) ? d.Place.City.Name : null,
                District = (d.Place.DistrictId.HasValue) ? d.Place.District.Name : null
            }).ToList();
            ViewBag.Name = region.RegionName;
            ViewBag.Id = region.Id;
            ViewBag.StartYear = startYear;
            ViewBag.EndYear = endYear;
            return View(model);
        }

        [HttpGet]
        public ActionResult View(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Types", "Classifications");
            }
            EventDTO ev = _eventService.GetEvent(id.Value);
            var model = new EventViewModel()
            {
                Id = ev.Id,
                Info = ev.Info,
                Region = ev.Place.Region.RegionName,
                EventName = ev.EventName,
                Event = ev.EventTypeCode + " " + ev.EventType.Name,
                EventDate = ev.EventDate,
                District = (ev.Place.DistrictId.HasValue) ? ev.Place.District.Name : null,
                Address = ev.Place.Address,
                City = (ev.Place.CityId.HasValue) ? ev.Place.City.Name : null,
                SavedDiagrams = ev.SavedDiagrams.Select(s => new SavedDiagramViewModel()
                {
                    Date = s.Date.ToString(),
                    ExpertCheck = s.ExpertCheck ? "Так" : "Ні",
                    Id = s.Id,
                    EventId = s.EventId,
                    DiagramType = s.DiagramType,
                    DiagramUpdates = s.DiagramUpdates.Select(x => new DiagramUpdateViewModel()
                    {
                        Date = x.Date.ToString(),
                        Id = x.Id,
                        JsonDiagram = x.JsonDiagram,
                        SavedDiagramId = x.SavedDiagramId,
                        Updates = x.Updates,
                        UserName = x.User.Name,
                        UserRole = x.User.Role.Name
                    }).OrderByDescending(x => x.Date)
                }).ToList()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Event(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Types", "Classifications");
            }
            var diagrams = _eventService.GetEventsByCode(id.Value);
            var eventType = _eventTypeService.GetEventType(id.Value);
            var model = diagrams.Select(d => new EventViewModel()
            {
                Id = d.Id,
                Event = d.EventTypeCode.ToString() + " " + d.EventType.Name,
                EventDate = d.EventDate,
                EventName = d.EventName,
                Info = d.Info,
                Region = d.Place.Region.RegionName,
                Address = d.Place.Address,
                City = (d.Place.CityId.HasValue) ? d.Place.City.Name : null,
                District = (d.Place.DistrictId.HasValue) ? d.Place.District.Name : null
            }).ToList();
            ViewBag.Name = eventType.Name;
            ViewBag.Code = eventType.Code;
            return View(model);
        }

        [Authorize(Roles = "Адміністратор")]
        [HttpGet]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ev = _eventService.GetEvent(id.Value);
            var eventModel = new EventViewModel()
            {
                Id = ev.Id,
                Info = ev.Info,
                Region = ev.Place.Region.RegionName,
                EventName = ev.EventName,
                Event = ev.EventTypeCode + " " + ev.EventType.Name,
                EventDate = ev.EventDate
            };
            return View(eventModel);
        }

        [Authorize(Roles = "Адміністратор")]
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            _eventService.DeleteEvent(id);
            return RedirectToAction("Index", "Default");
        }

        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var d = _eventService.GetEvent(id.Value);
            var model = new AddEventViewModel()
            {
                Guid = d.Id,
                Info = d.Info,
                Date = d.EventDate,
                EventTypeCode = d.EventTypeCode,
                RegionId = d.Place.RegionId,
                Name = d.EventName,
                DistrictId = d.Place.DistrictId,
                CityId = d.Place.CityId,
                Address = d.Place.Address
            };
            model.Districts = new SelectList(_districtService.GetDistrictsForRegion(model.RegionId), "Id", "Name");
            model.EventTypes = new SelectList(
                    _eventTypeService.GetAllEventTypes()
                        .Select(e => new { Code = e.Code, Name = e.Code.ToString() + " " + e.Name }), "Code", "Name");
            model.Regions = new SelectList(_regionService.GetAllRegions(), "Id", "RegionName");
            if (model.DistrictId.HasValue)
            {
                model.Cities = new SelectList(_cityService.GetCitiesForDistrict(model.DistrictId.Value), "Id", "Name");
            }
            else
            {
                model.Cities = new List<SelectListItem>();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(AddEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldEvent = _eventService.GetEvent(model.Guid.Value);
                oldEvent.EventDate = model.Date;
                oldEvent.EventName = model.Name;
                oldEvent.EventTypeCode = model.EventTypeCode;
                oldEvent.Info = model.Info;
                oldEvent.Place.Address = model.Address;
                oldEvent.Place.CityId = model.CityId;
                oldEvent.Place.DistrictId = model.DistrictId;
                oldEvent.Place.RegionId = model.RegionId;
                _eventService.EditEvent(oldEvent);
                return RedirectToAction("View", "Events", new { id = model.Guid.Value });
            }
            model.Districts = new SelectList(_districtService.GetDistrictsForRegion(model.RegionId), "Id", "Name");
            model.EventTypes = new SelectList(
                _eventTypeService.GetAllEventTypes()
                    .Select(e => new { Code = e.Code, Name = e.Code.ToString() + " " + e.Name }), "Code", "Name");
            model.Regions = new SelectList(_regionService.GetAllRegions(), "Id", "RegionName");
            if (model.DistrictId.HasValue)
            {
                model.Cities = new SelectList(_cityService.GetCitiesForDistrict(model.DistrictId.Value), "Id", "Name");
            }
            else
            {
                model.Cities = new List<SelectListItem>();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetDistricts(int regionId)
        {
            IEnumerable<DistrictDTO> districts;
            try
            {
                districts = _districtService.GetDistrictsForRegion(regionId);
            }
            catch (Exception e)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(districts, "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCities(int districtId)
        {
            IEnumerable<CityDTO> cities;
            try
            {
                cities = _cityService.GetCitiesForDistrict(districtId);
            }
            catch (Exception e)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(cities, "Id", "Name"), JsonRequestBehavior.AllowGet);
        }
    }
}