namespace AccountManagement.Exceptions
{
    public abstract class ConflictException : CustomException
    {
        protected ConflictException(string message) : base(message)
        {
        }
    }
}