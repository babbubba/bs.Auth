namespace bs.Auth.Api
{
    public class Response<T>
    {
        public Response(T result)
        {
            Result = result;
            Success = true;
        }
        public bool Success { get; set; }
        public bool Warn { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public T Result { get; set; }
        public string WarnMessage { get; set; }

    }
}
