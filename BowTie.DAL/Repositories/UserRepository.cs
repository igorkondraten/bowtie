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
    public class UserRepository : IUserRepository
    {
        private readonly IDataContext db;

        public UserRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Set<User>().ToList();
        }

        public User Get(int id)
        {
            return db.Set<User>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(User item)
        {
            db.Set<User>().Add(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User user = db.Set<User>().Find(id);
            if (user != null)
                db.Set<User>().Remove(user);
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> expression)
        {
            return db.Set<User>().Where(expression).ToList();
        }
    }
}
