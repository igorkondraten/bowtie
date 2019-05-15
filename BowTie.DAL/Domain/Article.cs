namespace BowTie.DAL.Domain
{
    public class Article
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public string Content { get; set; }
        public int? SubArticleId { get; set; }
        public virtual Article SubArticle { get; set; }
    }
}
