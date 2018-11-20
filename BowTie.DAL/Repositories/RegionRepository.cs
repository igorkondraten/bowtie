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
    public class RegionRepository : IRegionRepository
    {
        private BowTieContext db;
        public RegionRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<Region> GetAll()
        {
            return db.Regions;
        }

        public Region Get(int id)
        {
            return db.Regions.SingleOrDefault(d => d.Id == id);
        }

        public void Create(Region item)
        {
            db.Regions.Add(item);
        }

        public void Update(Region item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Region Region = db.Regions.Find(id);
            if (Region != null)
                db.Regions.Remove(Region);
        }
    }
}
