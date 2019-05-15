using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces.Repositories;
using System.Linq.Expressions;
using BowTie.DAL.Interfaces;

namespace BowTie.DAL.Repositories
{
    public class DiagramUpdateRepository : IDiagramUpdateRepository
    {
        private readonly IDataContext db;

        public DiagramUpdateRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<DiagramUpdate> GetAll()
        {
            return db.Set<DiagramUpdate>().OrderByDescending(s => s.Date).ToList();
        }

        public DiagramUpdate Get(int id)
        {
            return db.Set<DiagramUpdate>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(DiagramUpdate item)
        {
            db.Set<DiagramUpdate>().Add(item);
        }

        public void Update(DiagramUpdate item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            DiagramUpdate diagramUpdate = db.Set<DiagramUpdate>().Find(id);
            if (diagramUpdate != null)
                db.Set<DiagramUpdate>().Remove(diagramUpdate);
        }

        public IEnumerable<DiagramUpdate> Find(Expression<Func<DiagramUpdate, bool>> expression)
        {
            return db.Set<DiagramUpdate>().Where(expression).OrderByDescending(s => s.Date).ToList();
        }
    }
}
