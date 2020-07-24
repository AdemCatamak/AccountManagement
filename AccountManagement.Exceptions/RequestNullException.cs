namespace AccountManagement.Exceptions
{
    public class RequestNullException : ValidationException
    {
        public RequestNullException() : base(string.Empty)
        {
        }
    }
}