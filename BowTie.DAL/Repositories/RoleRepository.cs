using System.Collections.Generic;
using System.Data.Entity;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;
using BowTie.DAL.Interfaces.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BowTie.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDataContext db;

        public RoleRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<Role> GetAll()
        {
            return db.Set<Role>().ToList();
        }

        public Role Get(int id)
        {
            return db.Set<Role>().Find(id);
        }

        public void Create(Role item)
        {
            db.Set<Role>().Add(item);
        }

        public void Update(Role item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Role role = db.Set<Role>().Find(id);
            if (role != null)
                db.Set<Role>().Remove(role);
        }

        public IEnumerable<Role> Find(Expression<Func<Role, bool>> expression)
        {
            return db.Set<Role>().Where(expression).ToList();
        }
    }
}
