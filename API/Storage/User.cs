

namespace OnlineStore
{
    public class User
    {

        public static string? AdminUsername { get; set; }
        public static string? AdminPassword { get; set; }
        public static string? AdminId { get; set; }

        public static string? LastLogin { get; set; }
        public static string? Token { get; set; }

        internal static void SetAdminCredential(string username, string password)
        {
            AdminUsername = username;
            AdminPassword = password;
            AdminId = Guid.NewGuid().ToString();
        }


        public static string Login(LoginCriteria login)
        {

            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return "";
            }

            if (AdminUsername == login.Username && SecurePassword.Verify(login.Password, AdminPassword))
            {
                string token = Authentication.Encrypt($"{AdminId}##{login.Username}##{Helper.DateTimeToChar14(DateTime.UtcNow)}");
                LastLogin = Helper.DateTimeToChar14(DateTime.UtcNow);
                Token = token;
                return token;
            }

            return "";
        }

        internal static bool IsLogin()
        {
            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(LastLogin))
            {
                return false;
            }
            var tokenDate = Helper.Char14ToDateTime(LastLogin, DateTime.MinValue);

            if (tokenDate.AddHours(3) > DateTime.UtcNow)
            {
                return true;
            }
            else
            {
                Logout();
                return false;
            }
        }

        internal static bool Logout()
        {
            try
            {
                LastLogin = null;
                Token = null;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
