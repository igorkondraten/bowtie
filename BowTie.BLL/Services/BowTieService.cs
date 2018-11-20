using System;
using System.Collections.Generic;
using System.Linq;
using BowTie.BLL.Mappers;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Repositories;
using BowTie.DAL.Domain;

namespace BowTie.BLL.Services
{
    public class BowTieService : IBowTieService
    {
        UnitOfWork db;
        public BowTieService()
        {
            db = new UnitOfWork();
        }

        // Метод для розрахунку кількості збережених діаграм по розділах класифікатора
        private int GetTreeSum(List<EventTypeDTO> e, int parentId)
        {
            var arr = e.Where(a => a.ParentCode.Equals(parentId)).ToList();
            int sum = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                int subEventCount = e.Where(a => a.ParentCode.Equals(arr[i].Code)).Count();
                if (subEventCount > 0)
                {
                    int c = GetTreeSum(e, arr[i].Code);
                    arr[i].Diagrams += c;
                    sum += arr[i].Diagrams;
                }
                else
                {
                    sum += arr[i].Diagrams;
                }
            }
            return sum;
        }

        public IEnumerable<EventTypeDTO> GetEventTypes()
        {
            var ev = db.EventTypes.GetAll().OrderBy(e => e.Code).Select(c => new EventTypeMapper().Map(c)).ToList();
            GetTreeSum(ev, 0);
            return ev;
        }

        public EventTypeDTO GetEvent(int code)
        {
            return new EventTypeMapper().Map(db.EventTypes.Get(code));
        }
        public RegionDTO GetRegion(int id)
        {
            var region = new RegionMapper().Map(db.Regions.Get(id));
            region.Diagrams = db.Diagrams.CountDiagramsByRegion(id, null, null);
            return region;
        }

        public IEnumerable<RegionDTO> GetRegions(int? startYear, int? endYear)
        {
            var regions = db.Regions.GetAll().ToList().Select(r => new RegionMapper().Map(r)).ToList();
            for (int i = 0; i < regions.Count; i++)
            {
                regions[i].Diagrams = db.Diagrams.CountDiagramsByRegion(regions[i].Id, startYear, endYear);
            }
            return regions;
        }
        public IEnumerable<RegionDTO> GetRegions()
        {
            var regions = db.Regions.GetAll().ToList().Select(r => new RegionMapper().Map(r)).ToList();
            for (int i = 0; i < regions.Count; i++)
            {
                regions[i].Diagrams = db.Diagrams.CountDiagramsByRegion(regions[i].Id, null, null);
            }
            return regions;
        }

        public IEnumerable<DistrictDTO> GetDistricts(int regionId)
        {
            return db.Districts.GetByRegion(regionId).Select(d => new DistrictMapper().Map(d)).ToList();
        }
        public IEnumerable<CityDTO> GetCities(int districtId)
        {
            return db.Cities.GetByDistrict(districtId).Select(d => new CityMapper().Map(d)).ToList();
        }

        public IEnumerable<DiagramDTO> GetDiagramsByEvent(int eventCode)
        {
            var childEvents = db.EventTypes.GetChildren(db.EventTypes.GetAll().ToList(), eventCode).Select(e => e.Code).ToList();
            childEvents.Add(eventCode);
            List<DiagramDTO> diagrams = new List<DiagramDTO>();
            foreach(var e in childEvents)
            {
                foreach (var diagram in db.Diagrams.GetDiagramsByEvent(e).ToList().Select(d => new DiagramMapper().Map(d)))
                {
                    diagrams.Add(diagram);
                }
            }
            return diagrams.OrderByDescending(d => d.EventDate);
        }

        // Метод для отримання статистики по типах техногенних НС для одного регіону за вибрані роки
        // Повертає анонімний тип з полями Name - назва типу техногенної НС, Count - кількість НС
        public IEnumerable<dynamic> GetStats(int regionId, int? startYear, int? endYear)
        {
            // Завантаження діаграм для регіону за вибрані роки
            var diagramsByRegionYear = db.Diagrams.GetDiagramsByRegion(regionId, startYear, endYear).ToList();

            // Завантаження типів техногенних ситуацій для яких обраховується статистика
            var events = db.EventTypes.GetNearChildrenEvents(10000);
            // Створення пустого списку з анонімним типом і полями Name, Count
            var diagrams = new[] { new { Name = "", Count = 0 } }.Skip(1).ToList();

            foreach(var e in events)
            {
                int count = 0;
                var childEvents = db.EventTypes.GetChildren(db.EventTypes.GetTechnogenEvents().ToList(), e.Code).Select(ev => ev.Code).ToList();
                childEvents.Add(e.Code);
                foreach (var ev in childEvents)
                {
                    count += diagramsByRegionYear.Where(d => d.EventTypeCode == ev).Count();
                }
                diagrams.Add(new { Name = e.Name, Count = count });
            }
            return diagrams;  
        }

        // Метод для отримання списку діаграм для заданого регіону по роках починаючи з startYear, закінчуючи endYear
        public IEnumerable<DiagramDTO> GetDiagramsByRegion(int regionId, int? startYear, int? endYear)
        {
            return db.Diagrams.GetDiagramsByRegion(regionId, startYear, endYear).Select(d => new DiagramMapper().Map(d)).ToList();
        }

        public Guid CreateDiagram(int eventCode, int regionId, DateTime date, string info, string name, int? districtId, int? cityId, string adress)
        {
            if (db.EventTypes.Get(eventCode) == null) throw new ArgumentException("Не знайдено події у класифікаторі!");
            if (db.Regions.Get(regionId) == null) throw new ArgumentException("Область не знайдена!");
            if (info != null && info.Length > 50000) throw new ArgumentException("Довжина рядка перевищує 50000 символів!");
            if (name.Length > 80) throw new ArgumentException("Довжина назви перевищує 80 символів!");
            if (adress != null && adress.Length > 100) throw new ArgumentException("Довжина адреси перевищує 100 символів!");
            Place place = new Place() { RegionId = regionId, DistrictId = districtId, CityId = cityId, Adress = adress };
            db.Places.Create(place);
            Diagram newDiagram = new Diagram() { EventTypeCode = eventCode, EventDate = date, Info = info, ExpertCheck = false, EventName = name, PlaceId = place.Id };
            db.Diagrams.Create(newDiagram);
            db.Save();
            return newDiagram.Id;
        }

        public void EditDiagramInfo(Guid diagramId, int eventCode, int regionId, DateTime date, string info, string name, int? cityId, int? districtId, string adress)
        {
            var diagram = db.Diagrams.Get(diagramId);
            if (diagram != null)
            {
                diagram.EventTypeCode = eventCode;
                diagram.EventDate = date;
                diagram.Info = info;
                diagram.EventName = name;
                db.Diagrams.Update(diagram);
                if (diagram.Place.RegionId != regionId || diagram.Place.CityId != cityId || diagram.Place.DistrictId != districtId)
                {
                    var place = db.Places.Get(diagram.PlaceId);
                    place.RegionId = regionId;
                    place.CityId = cityId;
                    place.DistrictId = districtId;
                    place.Adress = adress;
                    db.Places.Update(place);
                }
                db.Save();
            }
        }

        public void SaveDiagram(Guid diagramId, string json, int userId, bool expertCheck, string updates)
        {
            var diagram = db.Diagrams.Get(diagramId);
            if (diagram != null)
            {
                Save s = new Save() { Date = DateTime.Now, DiagramId = diagramId, JsonDiagram = json, UserId = userId, Updates = updates };
                db.Saves.Create(s);
                diagram.ExpertCheck = expertCheck;
                db.Save();
            }
        }

        public DiagramDTO GetDiagram(Guid id)
        {
            var diagram = db.Diagrams.Get(id);
            if (diagram != null)
            {
                return new DiagramMapper().Map(diagram);
            }
            else throw new ArgumentException("Діаграма не знайдена");
        }

        public void DeleteDiagram(Guid diagramId)
        {
            var diagram = db.Diagrams.Get(diagramId);
            if (diagram != null)
            {
                db.Diagrams.Delete(diagramId);
            } else
                throw new ArgumentException("Діаграма не знайдена");
            db.Save();
        }

        // Метод для отримання історії редагувань діаграми
        public IEnumerable<SaveDTO> GetSavesForDiagram(Guid diagramId)
        {
            return db.Saves.GetSavesForDiagram(diagramId).OrderByDescending(s => s.Date).ToList().Select(s => new SaveMapper().Map(s));
        }

        public SaveDTO GetSave(int save)
        {
            var s = db.Saves.Get(save);
            return new SaveMapper().Map(s);
        }

        public void DeleteSave(int id)
        {
            var save = db.Saves.Get(id);
            if (save != null)
            {
                db.Saves.Delete(id);
                db.Save();
            }
            else
                throw new ArgumentException("Діаграма не знайдена");
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
