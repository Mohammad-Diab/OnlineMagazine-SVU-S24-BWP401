using System.Data.SqlClient;

namespace OnlineStore
{
    public class Paragraph
    {
        public string Id { get; set; }
        public string ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageURL { get; set; }

        public int Position { get; set; }

        public string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public string? DeletedAt { get; set; }


        static string BaseQuery = "SELECT ID, TITLE, ARTICLE_ID, P_CONTENT, IMAGE_URL, POSITION, CREATED_AT, UPDATED_AT, DELETED_AT FROM TBL_PARAGRAPHS ";

        public Paragraph()
        {
            Id = "";
            ArticleId = "";
            Position = 0;
            CreatedAt = "";
        }

        public Paragraph(string articleId, int position, string title = "", string content = "", string imageUrl = "", string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            ArticleId = articleId;
            Position = position;
            Title = title;
            Content = content;
            ImageURL = imageUrl;
            CreatedAt = Helper.DateTimeToChar14(DateTime.UtcNow);
        }

        public Paragraph(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            ArticleId = reader[reader.GetOrdinal("ARTICLE_ID")].ToString() ?? "";
            Title = reader[reader.GetOrdinal("TITLE")].ToString() ?? "";
            Content = reader[reader.GetOrdinal("P_CONTENT")].ToString() ?? "";
            ImageURL = reader[reader.GetOrdinal("IMAGE_URL")].ToString() ?? "";
            Position = DatabaseConnection.ConvertToInt32(reader[reader.GetOrdinal("POSITION")]);
            CreatedAt = reader[reader.GetOrdinal("CREATED_AT")].ToString() ?? "";
            UpdatedAt = reader[reader.GetOrdinal("UPDATED_AT")].ToString() ?? null;
            DeletedAt = reader[reader.GetOrdinal("DELETED_AT")].ToString() ?? null;
        }


        internal static List<Paragraph> GetArticleParagraphesList(string articleId)
        {
            var paragraphes = new List<Paragraph>();
            string whereStatment = " WHERE ARTICLE_ID = @0 and DELETED_AT IS NULL ";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereStatment, articleId);
            while (reader?.Read() ?? false)
            {
                Paragraph paragraph = new Paragraph(reader); 
                paragraphes.Add(paragraph);
            }
            reader?.Close();
            return paragraphes;
        }

        internal int Insert()
        {
            string query = "Insert into TBL_PARAGRAPHS (ID, ARTICLE_ID, TITLE, P_CONTENT, IMAGE_URL, POSITION, CREATED_AT) VALUES (@0, @1, @2, @3, @4, @5, @6)";
            int result = DatabaseConnection.NonQuery(query, Id, ArticleId, Title ?? "", Content ?? "", ImageURL ?? "", Position, CreatedAt);
            return result;
        }

        internal int Delete()
        {
            string query = "UPDATE TBL_PARAGRAPHS SET DELETED_AT = @0 WHERE ID = @1";
            int result = DatabaseConnection.NonQuery(query, Helper.DateTimeToChar14(DateTime.UtcNow), Id);
            return result;
        }
    }
}
