namespace AccountManagement.Exceptions
{
    public abstract class NotFoundException : CustomException
    {
        protected NotFoundException(string message) : base(message)
        {
        }
    }
}