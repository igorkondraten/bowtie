using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces.Repositories;
using System;
using System.Linq.Expressions;
using BowTie.DAL.Interfaces;

namespace BowTie.DAL.Repositories
{
    public class EventTypeRepository : IEventTypeRepository
    {
        private readonly IDataContext db;

        public EventTypeRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<EventType> GetAll()
        {
            return db.Set<EventType>().OrderBy(e => e.Code).ToList();
        }
        
        public EventType Get(int id)
        {
            return db.Set<EventType>().SingleOrDefault(d => d.Code == id);
        }

        public void Create(EventType item)
        {
            db.Set<EventType>().Add(item);
        }

        public void Update(EventType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            EventType eventType = db.Set<EventType>().Find(id);
            if (eventType != null)
                db.Set<EventType>().Remove(eventType);
        }

        public IEnumerable<EventType> Find(Expression<Func<EventType, bool>> expression)
        {
            return db.Set<EventType>().Where(expression).OrderBy(e => e.Code).ToList();
        }
    }
}
