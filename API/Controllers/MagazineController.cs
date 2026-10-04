using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Reflection;

namespace OnlineStore.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MagazineController : MyControllerBase
    {

        [HttpGet]
        [ActionName(nameof(GetNewArticles))]
        public ApiResponse<List<ArticleGrid>> GetNewArticles()
        {
            try
            {
                var result = Logic.GetNewArticles();
                return new ApiResponse<List<ArticleGrid>>(result);
            }
            catch (Exception ex) { return new ApiResponse<List<ArticleGrid>>(400, ex.Message); }
        }


        [HttpGet]
        [ActionName(nameof(GetTopArticles))]
        public ApiResponse<List<ArticleGrid>> GetTopArticles()
        {
            try
            {
                var result = Logic.GetTopArticles();
                return new ApiResponse<List<ArticleGrid>>(result);
            }
            catch (Exception ex) { return new ApiResponse<List<ArticleGrid>>(400, ex.Message); }
        }


        [HttpGet]
        [ActionName(nameof(GetImage))]
        public ApiResponse<string> GetImage(string path)
        {
            try
            {
                Thread.Sleep(250);
                string result = Logic.GetImage(path);
                return new ApiResponse<string>(result);
            }
            catch (Exception)
            {
                return new ApiResponse<string>("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAQklEQVR4AWMgGKy8vLy0tLVbMPDw9XcjIyMjAwMDOxETDw9Xcjk5OTU0tDS0gRI3aSYZaXRB8oAAAAASUVORK5CYII=");
            }
        }


        [HttpGet, ActionName(nameof(GetArticles))]
        public ApiResponse<List<ArticleGrid>> GetArticles()
        {
            try
            {
                var result = Logic.GetArticles();
                return new ApiResponse<List<ArticleGrid>>(result);
            }
            catch (Exception ex) { return new ApiResponse<List<ArticleGrid>>(400, ex.Message); }
        }


        [HttpGet]
        [ActionName(nameof(GetArticle))]
        public ApiResponse<ArticleDetail> GetArticle(string articleId)
        {
            try
            {
                var result = Logic.GetArticle(articleId);
                return new ApiResponse<ArticleDetail>(result);
            }
            catch (Exception ex) { return new ApiResponse<ArticleDetail>(400, ex.Message); }
        }


        [HttpGet]
        [ActionName(nameof(DeleteArticle))]
        public ApiResponse<bool> DeleteArticle(string articleId)
        {
            try
            {
                bool isLoggedin = Authentication.IsLogin(getTokenValue());
                if (!isLoggedin)
                {
                    return new ApiResponse<bool>(403, "يرجى تسجيل الدخول");
                }
                bool result = Logic.DeleteArticle(articleId);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }


        [HttpPost]
        [ActionName(nameof(CreateArticle))]
        public async Task<ApiResponse<bool>> CreateArticle([FromForm] ArticleTable article)
        {
            try
            {
                bool isLoggedin = Authentication.IsLogin(getTokenValue());
                if (!isLoggedin)
                {
                    return new ApiResponse<bool>(403, "يرجى تسجيل الدخول");
                }

                if (article.Image == null || article.Image.Length == 0)
                {
                    return new ApiResponse<bool>(400, "لم تقم بتحميل صورة");
                }

                if (article.Image.Length > 524288)
                {
                    return new ApiResponse<bool>(400, "الصورة كبيرة جدا... الحجم الأقصى للملف هو 512 كيلوبايت");
                }

                for(int ind = 0; ind < article.Paragraphes.Count; ind++)
                {
                    var fileKey = $"paragraphes[{ind}][image]";
                    var img = Request.Form.Files[fileKey];
                    article.Paragraphes[ind].Image = img;
                }

                if (string.IsNullOrEmpty(article.Title) || article.Paragraphes.Count == 0)
                {
                    return new ApiResponse<bool>(400, "الطلب غير مكتمل... يرجى ادخال العنوان وفقرة واحدة على الأقل");
                }

                string result = await Logic.CreateArticle(article);
                if (string.IsNullOrEmpty(result))
                {
                    return new ApiResponse<bool>(true);
                }
                else
                {
                    return new ApiResponse<bool>(400, result);
                }

            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }


        [HttpGet, ActionName(nameof(GetMessages))]
        public ApiResponse<List<MessageGrid>> GetMessages()
        {
            try
            {
                bool isLoggedin = Authentication.IsLogin(getTokenValue());
                if (!isLoggedin)
                {
                    return new ApiResponse<List<MessageGrid>>(403, "يرجى تسجيل الدخول");
                }
                var result = Logic.GetMessages();
                return new ApiResponse<List<MessageGrid>>(result);
            }
            catch (Exception ex) { return new ApiResponse<List<MessageGrid>>(400, ex.Message); }
        }

        [HttpGet]
        [ActionName(nameof(GetMessage))]
        public ApiResponse<string> GetMessage(string messageId)
        {
            try
            {
                bool isLoggedin = Authentication.IsLogin(getTokenValue());
                if (!isLoggedin)
                {
                    return new ApiResponse<string>(403, "يرجى تسجيل الدخول");
                }
                var result = Logic.GetMessage(messageId);
                return new ApiResponse<string>(result);
            }
            catch (Exception ex) { return new ApiResponse<string>(400, ex.Message); }
        }

        [HttpGet]
        [ActionName(nameof(DeleteMessage))]
        public ApiResponse<bool> DeleteMessage(string messageId)
        {
            try
            {
                bool isLoggedin = Authentication.IsLogin(getTokenValue());
                if (!isLoggedin)
                {
                    return new ApiResponse<bool>(403, "يرجى تسجيل الدخول");
                }
                bool result = Logic.DeleteMessage(messageId);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }

        [HttpPost]
        [ActionName(nameof(SendMessage))]
        public ApiResponse<bool> SendMessage([FromBody] MessageTable message)
        {
            try
            {
                if (string.IsNullOrEmpty(message.Name) || string.IsNullOrEmpty(message.Email) || string.IsNullOrEmpty(message.Message))
                {
                    return new ApiResponse<bool>(400, "الطلب غير مكتمل... يرجى ادخال اسمك وعنوان البريد الالكتروني والرسالة التي تود ارسالها");
                }

                bool result = Logic.SendMessage(message);
                if (result)
                {
                    return new ApiResponse<bool>(true);
                }
                else
                {
                    return new ApiResponse<bool>(400, "فشل ارسال الرسالة");
                }

            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }

    }
}
