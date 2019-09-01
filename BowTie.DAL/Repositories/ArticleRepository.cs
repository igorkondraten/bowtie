using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces.Repositories;
using System.Linq.Expressions;
using BowTie.DAL.Interfaces;

namespace BowTie.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IDataContext db;

        public ArticleRepository(IDataContext context)
        {
            db = context;
        }

        public IEnumerable<Article> GetAll()
        {
            return db.Set<Article>().OrderBy(x => x.Position).ToList();
        }

        public Article Get(int id)
        {
            return db.Set<Article>().SingleOrDefault(d => d.Id == id);
        }

        public void Create(Article item)
        {
            db.Set<Article>().Add(item);
        }

        public void Update(Article item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Article city = db.Set<Article>().Find(id);
            if (city != null)
                db.Set<Article>().Remove(city);
        }

        public IEnumerable<Article> Find(Expression<Func<Article, bool>> expression)
        {
            return db.Set<Article>().Where(expression).OrderBy(x => x.Position).ToList();
        }

        public IEnumerable<Article> GetAllTree()
        {
            return db.Set<Article>().Where(x => !x.ParentArticleId.HasValue).OrderBy(x => x.Position).ToList();
        }
    }
}
