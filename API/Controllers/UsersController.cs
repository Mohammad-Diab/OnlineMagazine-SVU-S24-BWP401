using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace OnlineStore.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UsersController : MyControllerBase
    {

        [HttpGet]
        [ActionName(nameof(IsLogin))]
        public ApiResponse<bool> IsLogin()
        {
            try
            {
                string token = getTokenValue();
                var result = Authentication.IsLogin(token);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }

        [HttpGet]
        [ActionName(nameof(Login))]
        public ApiResponse<string> Login(string username, string password)
        {
            try
            {
                var token = Authentication.Login(username, password);
                if (string.IsNullOrEmpty(token))
                {
                    return new ApiResponse<string>(400, "خطأ في اسم المستخدم وكلمة المرور");
                }
                return new ApiResponse<string>(token);

            }
            catch (Exception) { return new ApiResponse<string>(400, "خطأ في اسم المستخدم وكلمة المرور"); }
        }

        [HttpGet]
        [ActionName(nameof(Logout))]
        public ApiResponse<bool> Logout()
        {
            string token = getTokenValue();
            Authentication.Logout(token);
            return new ApiResponse<bool>(true);
        }

    }
}
