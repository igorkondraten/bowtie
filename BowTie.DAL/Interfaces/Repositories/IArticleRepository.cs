using System.Collections.Generic;
using BowTie.DAL.Domain;

namespace BowTie.DAL.Interfaces.Repositories
{
    public interface IArticleRepository : IRepository<Article, int>
    {
        IEnumerable<Article> GetAllTree();
    }
}
