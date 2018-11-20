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
    public class UserRepository : IUserRepository
    {
        private BowTieContext db;
        public UserRepository(BowTieContext context)
        {
            this.db = context;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.Include(d => d.Role);
        }

        public User Get(int id)
        {
            return db.Users.Include(d => d.Role).SingleOrDefault(d => d.Id == id);
        }
        public User Get(string name)
        {
            return db.Users.Include(d => d.Role).SingleOrDefault(d => d.Name == name);
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }
    }
}
