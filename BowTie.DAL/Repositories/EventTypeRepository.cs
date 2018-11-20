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
    public class EventTypeRepository : IEventTypeRepository
    {
        private BowTieContext db;
        public EventTypeRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<EventType> GetAll()
        {
            return db.EventTypes.Include(d => d.Diagrams).Include(d => d.Parent);
        }

        public List<EventType> GetChildren(List<EventType> types, int id)
        {
            return types
                .Where(x => x.ParentCode == id)
                .Union(types.Where(x => x.ParentCode == id)
                    .SelectMany(y => GetChildren(types, y.Code))
                ).ToList();
        }

        public IEnumerable<EventType> GetTechnogenEvents()
        {
            return db.EventTypes.Where(e => e.Code < 20000);
        }

        public IEnumerable<EventType> GetNearChildrenEvents(int eventCode)
        {
            return db.EventTypes.Where(e => e.ParentCode == eventCode);
        }

        public EventType Get(int id)
        {
            return db.EventTypes.Include(d => d.Diagrams).Include(d => d.Parent).SingleOrDefault(d => d.Code == id);
        }

        public void Create(EventType item)
        {
            db.EventTypes.Add(item);
        }

        public void Update(EventType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            EventType eventType = db.EventTypes.Find(id);
            if (eventType != null)
                db.EventTypes.Remove(eventType);
        }
    }
}
