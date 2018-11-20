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
    public class SaveRepository : ISaveRepository
    {
        private BowTieContext db;
        public SaveRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<Save> GetAll()
        {
            return db.Saves.Include(s => s.User).Include(s => s.User.Role).Include(s => s.Diagram);
        }

        public Save Get(int id)
        {
            return db.Saves.Include(s => s.User).Include(s => s.User.Role).Include(s => s.Diagram).SingleOrDefault(d => d.Id == id);
        }

        public IEnumerable<Save> GetSavesForDiagram(Guid diagramId)
        {
            return db.Saves.Include(s => s.User).Include(s => s.User.Role).Include(s => s.Diagram).Where(s => s.DiagramId == diagramId);
        }

        public void Create(Save item)
        {
            db.Saves.Add(item);
        }

        public void Update(Save item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Save save = db.Saves.Find(id);
            if (save != null)
                db.Saves.Remove(save);
        }
    }
}
