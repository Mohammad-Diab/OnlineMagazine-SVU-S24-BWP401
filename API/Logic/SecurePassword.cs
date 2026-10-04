
using System.Security.Cryptography;

namespace OnlineStore
{
    public static class SecurePassword
    {
        private const int SaltSize = 24;

        private const int HashSize = 32;

        public static string Hash(string password, int iterations)
        {
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return string.Format("$KODBYMOHAMMAD${0}${1}", iterations, base64Hash);
        }

        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$KODBYMOHAMMAD$");
        }

        public static bool Verify(string password, string? hashedPassword)
        {
            if(hashedPassword == null)
            {
                throw new NotSupportedException("لم يتم اعداد حساب المدير");
            }

            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("خوارزمية التشفير غير مدعومة");
            }

            var splittedHashString = hashedPassword.Replace("$KODBYMOHAMMAD$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
