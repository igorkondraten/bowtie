using System.Linq;
using System.Web.Mvc;
using BowTie.BLL.DTO;
using BowTie.BLL.Exceptions;
using BowTie.BLL.Interfaces;
using BowTie.View.Models;

namespace BowTie.View.Controllers
{
    public class ArticlesController : Controller
    {
        public IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public ActionResult List()
        {
            var articles = _articleService.GetAllArticles()
                .Select(x => new ArticleViewModel()
                {
                    Content = x.Content,
                    Id = x.Id,
                    Name = x.Name,
                    ParentArticleId = x.ParentArticleId,
                    State = new State() { Expanded = false },
                    Articles = x.ParentArticles.Any()
                        ? x.ParentArticles.Select(y => new ArticleViewModel()
                        {
                            Content = y.Content,
                            Id = y.Id,
                            Name = y.Name,
                            ParentArticleId = y.ParentArticleId
                        })
                        : null
                }).ToList();
            return View(articles);
        }

        [HttpGet]
        public ActionResult Add(int? id)
        {
            AddArticleViewModel model = new AddArticleViewModel();
            model.ParentArticleId = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AddArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new ArticleDTO()
                {
                    Content = model.Content,
                    Name = model.Name,
                    ParentArticleId = model.ParentArticleId
                };
                int id;
                try
                {
                    id = _articleService.CreateArticle(article);
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError("ParentArticle", e.Message);
                    return View(model);
                }
                return RedirectToAction("List", new { id = id });
            }
            return View(model);
        }
    }
}