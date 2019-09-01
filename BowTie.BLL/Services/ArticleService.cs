using System;
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
    public class ArticleService : IDisposable, IArticleService
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

        public void EditArticle(ArticleDTO article)
        {
            var oldArticle = db.Articles.Get(article.Id);
            if (oldArticle == null)
                throw new ValidationException("Article not found.");
            db.Articles.Update(Mapper.Map<ArticleDTO, Article>(article, oldArticle));
            db.Save();
        }

        public void DeleteArticle(int articleId)
        {
            var article = db.Articles.Get(articleId);
            if (article == null)
                throw new ValidationException("Article not found.");
            db.Articles.Delete(articleId);
            db.Save();
        }

        public IEnumerable<ArticleDTO> GetAllArticles()
        {
            return db.Articles.GetAllTree().Select(x => Mapper.Map<Article, ArticleDTO>(x));
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        
        #endregion
    }
}
