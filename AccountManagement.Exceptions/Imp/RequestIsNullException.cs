namespace AccountManagement.Exceptions.Imp
{
    public class RequestIsNullException : ValidationException
    {
        public RequestIsNullException() : base("Request should not be null")
        {
        }
    }
}