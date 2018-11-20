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
    public class DistrictRepository : IDistrictRepository
    {
        private BowTieContext db;
        public DistrictRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<District> GetAll()
        {
            return db.Districts.Include(d => d.Region);
        }

        public District Get(int id)
        {
            return db.Districts.Include(d => d.Region).SingleOrDefault(d => d.Id == id);
        }

        public IEnumerable<District> GetByRegion(int regionId)
        {
            return db.Districts.Where(d => d.RegionId == regionId);
        }

        public void Create(District item)
        {
            db.Districts.Add(item);
        }

        public void Update(District item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            District district = db.Districts.Find(id);
            if (district != null)
                db.Districts.Remove(district);
        }
    }
}
