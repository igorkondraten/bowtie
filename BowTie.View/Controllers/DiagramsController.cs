using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowTie.View.Models;
using BowTie.BLL.DTO;
using BowTie.BLL.Services;
using BowTie.BLL.Interfaces;
using System.Net;

namespace BowTie.View.Controllers
{
    public class DiagramsController : Controller
    {
        IBowTieService service = new BowTieService();
        IUserService userService = new UsersService();

        [Authorize]
        [HttpGet]
        public ActionResult Add(int? id)
        {
            AddDiagramViewModel model = new AddDiagramViewModel();
            IEnumerable<EventTypeDTO> eventTypes;
            IEnumerable<RegionDTO> regions;
            try
            {
                eventTypes = service.GetEventTypes();
                regions = service.GetRegions();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
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

        [HttpPost]
        public JsonResult GetDistricts(int regionId)
        {
            IEnumerable<DistrictDTO> districts;
            try
            {
                districts = service.GetDistricts(regionId);
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
                cities = service.GetCities(districtId);
            }
            catch (Exception e)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(cities, "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(AddDiagramViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid id = new Guid();
                try
                {
                    id = service.CreateDiagram(model.EventTypeCode, model.RegionId, model.Date, model.Info, model.Name, model.DistrictId, model.CityId, model.Adress);
                }
                catch (ArgumentException e)
                {
                    return new HttpStatusCodeResult(400, e.Message);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                return RedirectToAction("View", new { id = id });
            }
            IEnumerable<EventTypeDTO> eventTypes;
            IEnumerable<RegionDTO> regions;
            try
            {
                eventTypes = service.GetEventTypes();
                regions = service.GetRegions();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            model.EventTypes = new SelectList(eventTypes.ToList().Select(e => new { Code = e.Code, Name = e.Code.ToString() + " " + e.Name }), "Code", "Name");
            model.Regions = new SelectList(regions.ToList(), "Id", "RegionName");
            if (model.RegionId != 0)
            {
                IEnumerable<DistrictDTO> districts;
                try
                {
                    districts = service.GetDistricts(model.RegionId);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                model.Districts = new SelectList(districts.ToList(), "Id", "Name");
            } else
            {
                model.Districts = new List<SelectListItem>();
            }
            if (model.DistrictId.HasValue)
            {
                IEnumerable<CityDTO> cities;
                try
                {
                    cities = service.GetCities(model.DistrictId.Value);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                model.Cities = new SelectList(cities.ToList(), "Id", "Name");
            }
            else
            {
                model.Cities = new List<SelectListItem>();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult View(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Events", "Classifications");
            }
            DiagramViewModel model = new DiagramViewModel();
            DiagramDTO d;
            try
            {
                d = service.GetDiagram(id.Value);
            }
            catch (ArgumentException e)
            {
                return new HttpStatusCodeResult(404, e.Message);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            model.Id = d.Id;
            model.Info = d.Info;
            model.Region = d.Place.Region.RegionName;
            model.EventName = d.EventName;
            model.ExpertCheck = d.ExpertCheck ? "Так" : "Ні";
            model.Event = d.EventTypeCode.ToString() + " " + d.EventType.Name;
            model.EventDate = d.EventDate;
            if (d.Place.DistrictId.HasValue) model.District = d.Place.District.Name;
            model.Adress = d.Place.Adress;
            if (d.Place.CityId.HasValue) model.City = d.Place.City.Name;
            IEnumerable<SaveDTO> saves;
            try
            {
                saves = service.GetSavesForDiagram(id.Value);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            model.Saves = saves.Select(s => new SaveData() { Date = s.Date.ToString(), SaveId = s.Id, User = s.User.Name, UserRole = s.User.Role.Name, Update = s.Updates, Json = s.JsonDiagram }).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Event(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Events", "Classifications");
            }
            IEnumerable<DiagramDTO> diagrams;
            EventTypeDTO eventType;
            try
            {
                diagrams = service.GetDiagramsByEvent(id.Value);
                eventType = service.GetEvent(id.Value);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
            var model = diagrams.Select(d => new DiagramViewModel() { Id = d.Id, Event = d.EventTypeCode.ToString() + " " + d.EventType.Name, EventDate = d.EventDate, EventName = d.EventName, ExpertCheck = d.ExpertCheck ? "Так" : "Ні", Info = d.Info, Region = d.Place.Region.RegionName, Adress = d.Place.Adress, City = (d.Place.CityId.HasValue) ?  d.Place.City.Name : null, District = (d.Place.DistrictId.HasValue) ? d.Place.District.Name : null }).ToList();
            ViewBag.Name = eventType.Name;
            ViewBag.Code = eventType.Code;
            return View(model);
        }

        [HttpGet]
        public ActionResult Region(int? id, int? startYear, int? endYear)
        {
            if (id == null)
            {
                return RedirectToAction("Regions", "Classifications");
            }
            IEnumerable<DiagramDTO> diagrams;
            RegionDTO region;
            try
            {
                diagrams = service.GetDiagramsByRegion(id.Value, startYear, endYear);
                region = service.GetRegion(id.Value);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
            var model = diagrams.Select(d => new DiagramViewModel() { Id = d.Id, Event = d.EventTypeCode.ToString() + " " + d.EventType.Name, EventDate = d.EventDate, EventName = d.EventName, ExpertCheck = d.ExpertCheck ? "Так" : "Ні", Info = d.Info, Region = d.Place.Region.RegionName, Adress = d.Place.Adress, City = (d.Place.CityId.HasValue) ? d.Place.City.Name : null, District = (d.Place.DistrictId.HasValue) ? d.Place.District.Name : null }).ToList();
            ViewBag.Name = region.RegionName;
            ViewBag.Id = region.Id;
            ViewBag.StartYear = startYear;
            ViewBag.EndYear = endYear;
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
            DiagramViewModel model = new DiagramViewModel();
            DiagramDTO d;
            try
            {
                d = service.GetDiagram(id.Value);
            }
            catch (ArgumentException e)
            {
                return new HttpStatusCodeResult(404, e.Message);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
            model.Id = d.Id;
            model.Info = d.Info;
            model.Region = d.Place.Region.RegionName;
            model.EventName = d.EventName;
            model.ExpertCheck = d.ExpertCheck ? "Так" : "Ні";
            model.Event = d.EventTypeCode.ToString() + " " + d.EventType.Name;
            model.EventDate = d.EventDate;
            return View(model);
        }

        [Authorize(Roles = "Адміністратор")]
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                service.DeleteDiagram(id);
            }
            catch(ArgumentException e)
            {
                return new HttpStatusCodeResult(404, e.Message);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
            return RedirectToAction("Index","Default");
        }

        [Authorize(Roles = "Адміністратор")]
        [HttpPost]
        public ActionResult DeleteSave(int id)
        {
            try
            {
                service.DeleteSave(id);
            }
            catch (ArgumentException e)
            {
                return new HttpStatusCodeResult(404, e.Message);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddDiagramViewModel model = new AddDiagramViewModel();
            DiagramDTO d;
            try
            {
                d = service.GetDiagram(id.Value);
            }
            catch (ArgumentException e)
            {
                return new HttpStatusCodeResult(404, e.Message);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
            model.Guid = d.Id;
            model.Info = d.Info;
            model.ExpertCheck = d.ExpertCheck;
            model.Date = d.EventDate;
            model.EventTypeCode = d.EventTypeCode;
            model.RegionId = d.Place.RegionId;
            model.Name = d.EventName;
            model.DistrictId = d.Place.DistrictId;
            model.CityId = d.Place.CityId;
            model.Adress = d.Place.Adress;
            IEnumerable<DistrictDTO> districts;
            IEnumerable<EventTypeDTO> eventTypes;
            IEnumerable<RegionDTO> regions;
            try
            {
                districts = service.GetDistricts(model.RegionId);
                eventTypes = service.GetEventTypes();
                regions = service.GetRegions();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            model.Districts = new SelectList(districts.ToList(), "Id", "Name");
            model.EventTypes = new SelectList(eventTypes.ToList().Select(e => new { Code = e.Code, Name = e.Code.ToString() + " " + e.Name }), "Code", "Name");
            model.Regions = new SelectList(regions.ToList(), "Id", "RegionName");
            if (model.DistrictId.HasValue)
            {
                IEnumerable<CityDTO> cities;
                try
                {
                    cities = service.GetCities(model.DistrictId.Value);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                model.Cities = new SelectList(cities.ToList(), "Id", "Name");
            }
            else
            {
                model.Cities = new List<SelectListItem>();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(AddDiagramViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.EditDiagramInfo(model.Guid.Value, model.EventTypeCode, model.RegionId, model.Date, model.Info, model.Name, model.CityId, model.DistrictId, model.Adress);
                }
                catch (ArgumentException e)
                {
                    return new HttpStatusCodeResult(404, e.Message);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                return RedirectToAction("View", "Diagrams", new { id = model.Guid.Value });
            }
            IEnumerable<DistrictDTO> districts;
            IEnumerable<EventTypeDTO> eventTypes;
            IEnumerable<RegionDTO> regions;
            try
            {
                districts = service.GetDistricts(model.RegionId);
                eventTypes = service.GetEventTypes();
                regions = service.GetRegions();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            model.Districts = new SelectList(districts.ToList(), "Id", "Name");
            model.EventTypes = new SelectList(eventTypes.ToList().Select(e => new { Code = e.Code, Name = e.Code.ToString() + " " + e.Name }), "Code", "Name");
            model.Regions = new SelectList(regions.ToList(), "Id", "RegionName");
            if (model.DistrictId.HasValue)
            {
                IEnumerable<CityDTO> cities;
                try
                {
                    cities = service.GetCities(model.DistrictId.Value);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                model.Cities = new SelectList(cities.ToList(), "Id", "Name");
            }
            else
            {
                model.Cities = new List<SelectListItem>();
            }
            return View(model);       
        }

        [Authorize]
        [HttpPost]
        public ActionResult Save(SaveData data)
        {
            UserDTO user;
            try
            {
                user = userService.GetUser(User.Identity.Name);
                if (user != null)
                {
                    service.SaveDiagram(data.DiagramId, data.Json, user.Id, data.Checked, data.Update);
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            return RedirectToAction("View", new { id = data.DiagramId });
        }
    }
}