using System.Collections.Generic;
using BowTie.DAL.Domain;

namespace BowTie.BLL.DTO
{
    public class ArticleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int? ParentArticleId { get; set; }
        public virtual IEnumerable<ArticleDTO> ParentArticles { get; set; }
    }
}
