namespace OnlineStore
{
    public class Files
    {
        public static string? UploadImagePath { get; set; }

        internal static void SetUploadImagePath(string path)
        {
            UploadImagePath = Path.Combine(AppContext.BaseDirectory, path);
        }

        internal static bool StoreImage(byte[] imageData, string imageName)
        {
            if (UploadImagePath == null)
            {
                throw new Exception("لم يتم تحديد مسار ترفيع الصور");
            }
            if (imageData.Length == 0 || string.IsNullOrEmpty(imageName))
            {
                throw new Exception("ملف الصورة غير صالح");
            }

            if (!Directory.Exists(UploadImagePath))
            {
                Directory.CreateDirectory(UploadImagePath);
            }

            var imagePath = Path.Combine(UploadImagePath, imageName);

            if (!File.Exists(imagePath))
            {
                using (Stream st = File.OpenWrite(imagePath))
                {
                    st.Write(imageData, 0, imageData.Length);
                }
            }

            return true;
        }

        internal static byte[] GetImage(string imageName)
        {
            if (UploadImagePath == null)
            {
                throw new Exception("لم يتم تحديد مسار ترفيع الصور");
            }
            if (!Directory.Exists(UploadImagePath))
            {
                Directory.CreateDirectory(UploadImagePath);
            }

            var imagePath = Path.Combine(UploadImagePath, imageName);

            if (!File.Exists(imagePath))
            {
                throw new Exception("لم نعثر على الصورة المختارة");
            }

            using (Stream st = File.OpenRead(imagePath))
            {
                byte[] result = new byte[st.Length];
                st.Read(result, 0, (int)st.Length);
                return result;
            }
        }

        internal static bool DeleteImage(string imageName)
        {
            if (UploadImagePath == null)
            {
                throw new Exception("لم يتم تحديد مسار ترفيع الصور");
            }

            if (!Directory.Exists(UploadImagePath))
            {
                Directory.CreateDirectory(UploadImagePath);
            }

            var imagePath = Path.Combine(UploadImagePath, imageName);

            if (File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
