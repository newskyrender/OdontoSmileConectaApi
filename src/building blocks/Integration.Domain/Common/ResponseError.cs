namespace Integration.Domain.Common
{
    public class ResponseError
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public List<string> Details { get; set; } = new List<string>();
        
        public ResponseError()
        {
        }
        
        public ResponseError(string message)
        {
            Message = message;
        }
        
        public ResponseError(string message, string errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }
}
