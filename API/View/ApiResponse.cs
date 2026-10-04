namespace OnlineStore
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public int Code { get; set; } = 200;
        public string ErrorMessage { get; set; } = string.Empty;

        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(int code, string errorMessage)
        {
            Data = default;
            Code = code;
            ErrorMessage = errorMessage;
        }
    }
}
