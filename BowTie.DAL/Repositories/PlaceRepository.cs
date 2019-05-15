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
    public class PlaceRepository : IPlaceRepository
    {
        private readonly IDataContext db;

        public PlaceRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<Place> GetAll()
        {
            return db.Set<Place>().ToList();
        }

        public Place Get(int id)
        {
            return db.Set<Place>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(Place item)
        {
            db.Set<Place>().Add(item);
        }

        public void Update(Place item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Place place = db.Set<Place>().Find(id);
            if (place != null)
                db.Set<Place>().Remove(place);
        }

        public IEnumerable<Place> Find(Expression<Func<Place, bool>> expression)
        {
            return db.Set<Place>().Where(expression).ToList();
        }
    }
}
