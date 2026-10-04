namespace OnlineStore
{
    public static class Config
    {
        public static string ApiUrl { get; private set; } = "http://localhost/API/";

        public static void setApiUrl(string url) {
            ApiUrl = url;
        }
    }
}
