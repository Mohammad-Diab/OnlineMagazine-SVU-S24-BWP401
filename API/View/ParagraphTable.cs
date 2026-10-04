namespace OnlineStore
{
    public class ParagraphTable
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public int Position { get; set; }
        public IFormFile? Image { get; set; }


        public ParagraphTable()
        {
            Title = "";
            Content = "";
            Position = 0;
        }

        public ParagraphTable(string title, int position, string? content)
        {
            Title = title;
            Content = content ?? "";
            Position = position;
        }
    }
}
