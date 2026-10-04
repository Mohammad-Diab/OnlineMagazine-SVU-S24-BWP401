using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace OnlineStore
{
    public static class Logic
    {

        internal static List<ArticleGrid> GetNewArticles()
        {

            List<Article> articles = Article.GetNewArticles(5);
            return articles.Select(x => new ArticleGrid(x)).ToList();
        }

        internal static List<ArticleGrid> GetTopArticles()
        {
            List<Article> articles = Article.GetTopArticles(5);
            return articles.Select(x => new ArticleGrid(x)).ToList();
        }
        internal static string GetImage(string imageName)
        {
            var imageBytes = Files.GetImage(imageName);
            return $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";
        }

        internal static List<ArticleGrid> GetArticles()
        {
            List<Article> articles = Article.GetArticlesList();
            return articles.Select(x => new ArticleGrid(x)).ToList();
        }

        internal static ArticleDetail GetArticle(string articleId)
        {
            Article? a = Article.GetById(articleId);
            if (a == null) throw new Exception("لم يتم العثور على المقال");
            a.LoadParagraphes();
            a.ReadCount += 1;
            a.UpdateReadCount();
            ArticleDetail result = new ArticleDetail(a);
            return result;
        }

        internal static bool DeleteArticle(string id)
        {
            Article? a = Article.GetById(id) ?? throw new Exception("لم يتم العثور على المقال المحدد");
            Files.DeleteImage(a.ImageURL);
            a.LoadParagraphes();
            a.Paragraphes.ForEach(p =>
            {
                if (!string.IsNullOrEmpty(p.ImageURL))
                {
                    Files.DeleteImage(p.ImageURL);
                }
                p.Delete();
            });
            return a.Delete() > 0;
        }

        internal async static Task<string> CreateArticle(ArticleTable article)
        {
            if (article.Image == null)
            {
                throw new Exception("لم يتم تحديد الصورة");
            }

            using var stream = new MemoryStream();
            await article.Image.CopyToAsync(stream);
            byte[] imageData = stream.ToArray();
            string fileEtension = Path.GetExtension(article.Image.FileName);

            string aId = Guid.NewGuid().ToString();
            Article a = new Article(article.Title, aId + fileEtension, aId);
            var result = a.Insert();
            if (result == 0)
            {
                throw new Exception("خطأ أثناء اضافة المقال");
            }
            if (!Files.StoreImage(imageData, a.ImageURL))
            {
                throw new Exception("خطأ أثناء حفظ الصورة");
            }

            var errorMessage = "";
            article.Paragraphes.ForEach(async (x) =>
            {
                string pId = Guid.NewGuid().ToString();
                string imageUrl = "";
                if (x.Image == null && x.Title == null && x.Content == null)
                {
                    errorMessage = "الرجاء تعبئة الحقول المطلوبة";
                }
                if (x.Image != null && x.Image.Length > 0)
                {
                    if (x.Image.Length > 524288)
                    {
                        errorMessage = "الصورة كبيرة جدا... الحجم الأقصى للملف هو 512 كيلوبايت";
                    }
                    string pFileEtension = Path.GetExtension(x.Image.FileName);
                    if (!string.IsNullOrEmpty(pFileEtension))
                    {
                        imageUrl = pId + pFileEtension;
                    }
                    using var pStream = new MemoryStream();
                    await x.Image.CopyToAsync(pStream);
                    byte[] pImageData = pStream.ToArray();
                    if (!Files.StoreImage(pImageData, imageUrl))
                    {
                        errorMessage = "خطأ أثناء حفظ الصورة";
                    }
                }

                Paragraph p = new Paragraph(aId, x.Position, x.Title ?? "", x.Content ?? "", imageUrl, pId);
                result = p.Insert();
                if (result == 0)
                {
                    errorMessage = "خطأ أثناء اضافة المقال";
                }

            });
            return errorMessage;
        }


        internal static List<MessageGrid> GetMessages()
        {
           var articles = ContactUs.GetMessagesList();
            return articles.Select(x => new MessageGrid(x)).ToList();
        }

        internal static string GetMessage(string messageId)
        {
            ContactUs? m = ContactUs.GetById(messageId);
            if (m == null) throw new Exception("لم يتم العثور على الرسالة");
            m.MarkAsRead();
            return m.Message;
        }

        internal static bool DeleteMessage(string messageId)
        {
            ContactUs? m = ContactUs.GetById(messageId);
            if (m == null)
            {
                throw new Exception("لم يتم العثور على الرسالة");
            }
            return m.Delete() > 0;
        }

        internal static bool SendMessage(MessageTable message)
        {
            ContactUs m = new ContactUs(message.Name, message.Email, message.Message);
            var result = m.Insert();
            return result > 0;
        }

    }
}
