using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BowTie.BLL.DTO;
using BowTie.BLL.Exceptions;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;

namespace BowTie.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork db;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public int CreateArticle(ArticleDTO article)
        {
            if (article.ParentArticleId.HasValue)
            {
                if (db.Articles.Get(article.ParentArticleId.Value) == null)
                    throw new ValidationException("ParentArticle not found.");
            }
            var newArticle = Mapper.Map<ArticleDTO, Article>(article);
            db.Articles.Create(newArticle);
            db.Save();
            return newArticle.Id;
        }

        public IEnumerable<ArticleDTO> GetAllArticles()
        {
            return db.Articles.GetAllTree().Select(x => Mapper.Map<Article, ArticleDTO>(x));
        }
    }
}
