namespace OnlineStore
{
    public class ArticleGrid
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string CreatedAt { get; set; }
        public int ReadCount { get; set; }


        public ArticleGrid(Article article)
        {
            Id = article.Id;
            Title = article.Title;
            Image = article.ImageURL;
            CreatedAt = article.CreatedAt;
            ReadCount = article.ReadCount;
        }
    }
}
