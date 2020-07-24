namespace AccountManagement.Exceptions.Imp
{
    public class EmailAlreadyRegisteredException : ConflictException
    {
        public EmailAlreadyRegisteredException(string email) : base($"{email} has already been registered")
        {
        }
    }
}