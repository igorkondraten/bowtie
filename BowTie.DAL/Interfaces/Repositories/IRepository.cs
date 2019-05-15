using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BowTie.DAL.Interfaces.Repositories
{
    public interface IRepository<T, T1> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        T Get(T1 id);
        void Create(T item);
        void Update(T item);
        void Delete(T1 id);
    }
}
