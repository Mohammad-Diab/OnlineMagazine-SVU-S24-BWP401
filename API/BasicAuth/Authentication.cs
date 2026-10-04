using System.Security.Cryptography;
using System.Text;

namespace OnlineStore
{
    public static class Authentication
    {

        private static string EncryptionKey = "0d895a26-aa56-4ee4-a3c7-77801c5b2e48";

        public static string Encrypt(string encryptString)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string Login(string username, string password)
        {
            return User.Login(new LoginCriteria(username, password));
        }


        public static bool IsLogin(string token)
        {
            var decryptedToken = Decrypt(token);
            var username = (!string.IsNullOrEmpty(decryptedToken) && decryptedToken.Contains("##"))
                ? decryptedToken.Split("##")[1]
                : "";
            return User.IsLogin();
        }


        public static void Logout(string token)
        {
            try
            {
                var username = GetUsername(token);
                User.Logout();
            }
            catch (Exception) { }
        }

        public static string GetUsername(string token)
        {
            if (IsLogin(token))
            {
                var username = Decrypt(token).Split("##")?[1] ?? "";
                return username;
            }
            return "";
        }


    }
}
