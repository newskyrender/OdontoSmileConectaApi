namespace Integration.Domain.Common
{
    public class ServiceBase
    {
        protected virtual ResponseError CreateError(string message)
        {
            return new ResponseError(message);
        }
        
        protected virtual ResponseError CreateError(string message, string errorCode)
        {
            return new ResponseError(message, errorCode);
        }
    }
}
