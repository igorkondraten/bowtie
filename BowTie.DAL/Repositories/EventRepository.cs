using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;
using BowTie.DAL.Interfaces.Repositories;
using System.Linq.Expressions;

namespace BowTie.DAL.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IDataContext db;

        public EventRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<Event> GetAll()
        {
            return db.Set<Event>().ToList();
        }

        public Event Get(Guid id)
        {
            return db.Set<Event>().SingleOrDefault(d => d.Id == id);
        }
        public int CountDiagramsByRegion(int regionId, int? startYear, int? endYear)
        {
            return db.Set<Event>()
                .Count(d => d.Place.RegionId == regionId &&
                            (!startYear.HasValue || d.EventDate.Year >= startYear.Value) &&
                            (!endYear.HasValue || d.EventDate.Year <= endYear.Value));
        }

        public void Create(Event item)
        {
            db.Set<Event>().Add(item);
        }

        public void Update(Event item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            Event @event = db.Set<Event>().Find(id);
            if (@event != null)
                db.Set<Event>().Remove(@event);
        }

        public IEnumerable<Event> Find(Expression<Func<Event, bool>> expression)
        {
            return db.Set<Event>().Where(expression).ToList();
        }
    }
}
