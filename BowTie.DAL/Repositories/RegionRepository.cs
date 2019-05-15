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
    public class RegionRepository : IRegionRepository
    {
        private readonly IDataContext db;

        public RegionRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<Region> GetAll()
        {
            return db.Set<Region>().ToList();
        }

        public Region Get(int id)
        {
            return db.Set<Region>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(Region item)
        {
            db.Set<Region>().Add(item);
        }

        public void Update(Region item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Region Region = db.Set<Region>().Find(id);
            if (Region != null)
                db.Set<Region>().Remove(Region);
        }

        public IEnumerable<Region> Find(Expression<Func<Region, bool>> expression)
        {
            return db.Set<Region>().Where(expression).ToList();
        }
    }
}
