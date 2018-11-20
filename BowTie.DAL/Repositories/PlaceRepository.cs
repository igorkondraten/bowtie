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
    public class PlaceRepository : IPlaceRepository
    {
        private BowTieContext db;
        public PlaceRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<Place> GetAll()
        {
            return db.Places.Include(d => d.Region).Include(d => d.District).Include(d => d.City);
        }

        public Place Get(int id)
        {
            return db.Places.Include(d => d.Region).Include(d => d.District).Include(d => d.City).SingleOrDefault(d => d.Id == id);
        }

        public void Create(Place item)
        {
            db.Places.Add(item);
        }

        public void Update(Place item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Place place = db.Places.Find(id);
            if (place != null)
                db.Places.Remove(place);
        }
    }
}
