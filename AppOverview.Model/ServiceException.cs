namespace AppOverview.Model
{
    public class ServiceException : Exception
    {
        public ServiceException() : base() { }
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
        public ServiceException(string message, string? errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
        public string? ErrorCode { get; set; } = null;
    }
}
