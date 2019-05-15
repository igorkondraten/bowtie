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
    public class CityRepository : ICityRepository
    {
        private readonly IDataContext db;

        public CityRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<City> GetAll()
        {
            return db.Set<City>().ToList();
        }

        public City Get(int id)
        {
            return db.Set<City>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(City item)
        {
            db.Set<City>().Add(item);
        }

        public void Update(City item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            City city = db.Set<City>().Find(id);
            if (city != null)
                db.Set<City>().Remove(city);
        }

        public IEnumerable<City> Find(Expression<Func<City, bool>> expression)
        {
            return db.Set<City>().Where(expression).ToList();
        }
    }
}
