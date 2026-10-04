namespace OnlineStore
{
    public class ParagraphDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public int Position { get; set; }

        public ParagraphDetail(Paragraph paragraph)
        {
            Id = paragraph.Id;
            Title = paragraph.Title ?? "";
            Image = paragraph.ImageURL ?? "";
            Content = paragraph.Content ?? "";
            Position = paragraph.Position;
        }
    }
}
