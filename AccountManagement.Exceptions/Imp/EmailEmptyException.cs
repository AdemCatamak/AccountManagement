namespace AccountManagement.Exceptions.Imp
{
    public class EmailEmptyException : ValidationException
    {
        public EmailEmptyException() : base($"Email is not valid")
        {
        }
    }
}