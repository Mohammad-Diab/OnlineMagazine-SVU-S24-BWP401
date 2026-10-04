namespace OnlineStore
{
    public class ArticleTable
    {
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
        public List<ParagraphTable> Paragraphes { get; set; }

        public ArticleTable()
        {
            Title = "";
            Paragraphes = new List<ParagraphTable>();
        }

        public ArticleTable(string title, List<ParagraphTable> paragraphs)
        {
            Title = title;
            Paragraphes = paragraphs;
        }
    }
}
