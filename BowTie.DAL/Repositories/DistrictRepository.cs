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
    public class DistrictRepository : IDistrictRepository
    {
        private readonly IDataContext db;

        public DistrictRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<District> GetAll()
        {
            return db.Set<District>().ToList();
        }

        public District Get(int id)
        {
            return db.Set<District>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(District item)
        {
            db.Set<District>().Add(item);
        }

        public void Update(District item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            District district = db.Set<District>().Find(id);
            if (district != null)
                db.Set<District>().Remove(district);
        }

        public IEnumerable<District> Find(Expression<Func<District, bool>> expression)
        {
            return db.Set<District>().Where(expression).ToList();
        }
    }
}
