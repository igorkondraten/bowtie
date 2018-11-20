using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BowTie.DAL.EF;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;

namespace BowTie.DAL.Repositories
{
    public class DiagramRepository : IDiagramRepository
    {
        private BowTieContext db;
        public DiagramRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<Diagram> GetAll()
        {
            return db.Diagrams.Include(d => d.Place).Include(d => d.EventType);
        }

        public Diagram Get(Guid id)
        {
            return db.Diagrams.Include(d => d.Place).Include(d => d.Place.City).Include(d => d.Place.District).Include(d => d.Place.Region).Include(d => d.EventType).SingleOrDefault(d => d.Id == id);
        }

        public IEnumerable<Diagram> GetDiagramsByEvent(int eventCode)
        {
            return db.Diagrams.Include(d => d.Place).Include(d => d.EventType).Include(d => d.Place.City).Include(d => d.Place.District).Include(d => d.Place.Region).Where(d => d.EventTypeCode == eventCode);
        }

        public int GetDiagramsCountByRegionEvent(int regionId, int? startYear, int? endYear, int eventCode)
        {
            return db.Diagrams.Include(d => d.Place).Where(d => d.Place.RegionId == regionId && d.EventTypeCode == eventCode && (!startYear.HasValue || d.EventDate.Year >= startYear.Value) && (!endYear.HasValue || d.EventDate.Year <= endYear.Value)).Count();
        }

        public IEnumerable<Diagram> GetDiagramsByRegion(int regionId, int? startYear, int? endYear)
        {
            return db.Diagrams.Include(d => d.Place).Include(d => d.EventType).Include(d => d.Place.City).Include(d => d.Place.District).Include(d => d.Place.Region).Where(d => d.Place.RegionId == regionId && (!startYear.HasValue || d.EventDate.Year >= startYear.Value) && (!endYear.HasValue || d.EventDate.Year <= endYear.Value));
        }

        public int CountDiagramsByRegion(int regionId, int? startYear, int? endYear)
        {
            return db.Diagrams.Include(d => d.Place).Where(d => d.Place.RegionId == regionId && (!startYear.HasValue || d.EventDate.Year >= startYear.Value) && (!endYear.HasValue || d.EventDate.Year <= endYear.Value)).Count();
        }

        public void Create(Diagram item)
        {
            db.Diagrams.Add(item);
        }

        public void Update(Diagram item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            Diagram diagram = db.Diagrams.Find(id);
            if (diagram != null)
                db.Diagrams.Remove(diagram);
        }
    }
}
