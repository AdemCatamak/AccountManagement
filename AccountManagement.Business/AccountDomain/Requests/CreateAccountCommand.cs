using AccountManagement.Exceptions.Imp;

namespace AccountManagement.Business.AccountDomain.Requests
{
    public class CreateAccountCommand
    {
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public CreateAccountCommand(string email, string firstName, string lastName)
        {
            email ??= string.Empty;
            Email = email == string.Empty
                        ? throw new EmailEmptyException()
                        : email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}