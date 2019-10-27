using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IArticleService
    {
        int CreateArticle(ArticleDTO article);
        IEnumerable<ArticleDTO> GetAllArticles();
    }
}
