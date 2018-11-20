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
    public class CityRepository : ICityRepository
    {
        private BowTieContext db;
        public CityRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<City> GetAll()
        {
            return db.Cities.Include(d => d.District).Include(d => d.District.Region);
        }

        public City Get(int id)
        {
            return db.Cities.Include(d => d.District).Include(d => d.District.Region).SingleOrDefault(d => d.Id == id);
        }

        public IEnumerable<City> GetByDistrict(int districtId)
        {
            return db.Cities.Where(d => d.DistrictId == districtId);
        }

        public void Create(City item)
        {
            db.Cities.Add(item);
        }

        public void Update(City item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            City city = db.Cities.Find(id);
            if (city != null)
                db.Cities.Remove(city);
        }
    }
}
