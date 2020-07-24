namespace AccountManagement.Exceptions
{
    public abstract class ValidationException : CustomException
    {
        protected ValidationException(string message) : base(message)
        {
        }
    }
}