using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineStore
{
    public class Article
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public int ReadCount { get; set; }
        public string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public string? DeletedAt { get; set; }

        public List<Paragraph> Paragraphes { get; set; }

        static string BaseQuery = "SELECT ID, TITLE, IMAGE_URL, READ_COUNT, CREATED_AT, UPDATED_AT, DELETED_AT FROM TBL_ARTICLES ";


        public Article()
        {
            Id = "";
            Title = "";
            ImageURL = "";
            ReadCount = 0;
            CreatedAt = "";
            UpdatedAt = null;
            DeletedAt = null;
            Paragraphes = new List<Paragraph>();
        }

        public Article(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            Title = reader[reader.GetOrdinal("TITLE")].ToString() ?? "";
            ImageURL = reader[reader.GetOrdinal("IMAGE_URL")].ToString() ?? "";
            ReadCount = DatabaseConnection.ConvertToInt32(reader[reader.GetOrdinal("READ_COUNT")]);
            CreatedAt = reader[reader.GetOrdinal("CREATED_AT")].ToString() ?? "";
            UpdatedAt = reader[reader.GetOrdinal("UPDATED_AT")].ToString() ?? null;
            DeletedAt = reader[reader.GetOrdinal("DELETED_AT")].ToString() ?? null;
            Paragraphes = new List<Paragraph>();
        }


        public Article(string title, string imageUrl, string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Title = title;
            ImageURL = imageUrl;
            ReadCount = 0;
            CreatedAt = Helper.DateTimeToChar14(DateTime.UtcNow);
            Paragraphes = new List<Paragraph>();
        }

        public static List<Article> GetArticlesList()
        {
            try
            {
                var articles = new List<Article>();
                var whereString = " WHERE DELETED_AT IS NULL ";
                SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString);
                while (reader?.Read() ?? false)
                {
                    Article article = new Article(reader);
                    articles.Add(article);
                }
                reader?.Close();
                return articles;
            }
            catch (Exception) { throw; }
        }

        public static List<Article> GetNewArticles(int count)
        {
            try
            {
                var articles = new List<Article>();
                var whereString = " WHERE DELETED_AT IS NULL ";
                string query = $"{BaseQuery} {whereString} ORDER BY CREATED_AT DESC;";
                SqlDataReader? reader = DatabaseConnection.Query(query);
                int i = count;
                while ((reader?.Read() ?? false) && i > 0)
                {
                    Article article = new Article(reader);
                    articles.Add(article);
                    i--;
                }
                reader?.Close();
                return articles;
            }
            catch (Exception) { throw; }
        }

        public static List<Article> GetTopArticles(int count)
        {
            try
            {
                var articles = new List<Article>();
                var whereString = " WHERE DELETED_AT IS NULL ";
                string query = $"{BaseQuery} {whereString} ORDER BY READ_COUNT DESC";
                SqlDataReader? reader = DatabaseConnection.Query(query);
                int i = count;
                while ((reader?.Read() ?? false) && i > 0)
                {
                    Article article = new Article(reader);
                    articles.Add(article);
                    i--;
                }
                reader?.Close();
                return articles;
            }
            catch (Exception) { throw; }
        }

        internal static Article? GetById(string articleId)
        {
            string whereString = $" WHERE ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, articleId);
            if (reader?.Read() ?? false)
            {
                Article article = new Article(reader);
                return article;
            }
            reader?.Close();
            return null;
        }

        internal void LoadParagraphes()
        {
            Paragraphes = Paragraph.GetArticleParagraphesList(Id);
        }

        internal int UpdateReadCount()
        {
            string query = "UPDATE TBL_ARTICLES SET READ_COUNT = READ_COUNT + 1 WHERE ID = @0";
            int result = DatabaseConnection.NonQuery(query, Id);
            return result;
        }

        internal int Insert()
        {
            string query = "Insert into TBL_ARTICLES (ID, TITLE, IMAGE_URL, READ_COUNT, CREATED_AT) VALUES (@0, @1, @2, @3, @4)";
            int result = DatabaseConnection.NonQuery(query, Id, Title, ImageURL, ReadCount, CreatedAt);
            return result;
        }

        internal int Delete()
        {
            string query = "UPDATE TBL_ARTICLES SET DELETED_AT = @0 WHERE ID = @1";
            int result = DatabaseConnection.NonQuery(query, Helper.DateTimeToChar14(DateTime.UtcNow), Id);
            return result;
        }
    }
}
