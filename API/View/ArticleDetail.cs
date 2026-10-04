namespace OnlineStore
{
    public class ArticleDetail : ArticleGrid
    {
        public List<ParagraphDetail> Paragraphes { get; set; }

        public ArticleDetail(Article article) : base(article)
        {
            Paragraphes = article.Paragraphes.Select(p => new ParagraphDetail(p)).ToList();
        }
    }
}
