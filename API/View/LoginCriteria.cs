namespace OnlineStore
{
    public class LoginCriteria
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginCriteria()
        {
            Username = "";
            Password = "password";
            RememberMe = false;
        }

        public LoginCriteria(string username, string password, bool rememberMe = false)
        {
            Username = username;
            Password = password;
            RememberMe = rememberMe;
        }
    }
}
