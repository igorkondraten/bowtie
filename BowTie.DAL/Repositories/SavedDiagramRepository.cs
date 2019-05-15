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
    public class SavedDiagramRepository : ISavedDiagramRepository
    {
        private readonly IDataContext db;

        public SavedDiagramRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<SavedDiagram> GetAll()
        {
            return db.Set<SavedDiagram>().ToList();
        }

        public SavedDiagram Get(int id)
        {
            return db.Set<SavedDiagram>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(SavedDiagram item)
        {
            db.Set<SavedDiagram>().Add(item);
        }

        public void Update(SavedDiagram item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            SavedDiagram savedDiagram = db.Set<SavedDiagram>().Find(id);
            if (savedDiagram != null)
                db.Set<SavedDiagram>().Remove(savedDiagram);
        }

        public IEnumerable<SavedDiagram> Find(Expression<Func<SavedDiagram, bool>> expression)
        {
            return db.Set<SavedDiagram>().Where(expression).ToList();
        }
    }
}
