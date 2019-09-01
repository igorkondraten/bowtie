using System.Collections.Generic;

namespace BowTie.DAL.Domain
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Position { get; set; }
        public int? ParentArticleId { get; set; }
        public virtual Article ParentArticle { get; set; }
        public virtual ICollection<Article> ParentArticles { get; set; }
    }
}
