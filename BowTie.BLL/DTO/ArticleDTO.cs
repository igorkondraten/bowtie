namespace BowTie.BLL.DTO
{
    public class ArticleDTO
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public string Content { get; set; }
        public int? SubArticleId { get; set; }
        public ArticleDTO SubArticle { get; set; }
    }
}
