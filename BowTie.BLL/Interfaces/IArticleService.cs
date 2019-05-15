using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IArticleService : IDisposable
    {
        int CreateArticle(ArticleDTO article);
        void EditArticle(ArticleDTO article);
        void DeleteArticle(int articleId);
        IEnumerable<ArticleDTO> GetAllArticles();
    }
}
