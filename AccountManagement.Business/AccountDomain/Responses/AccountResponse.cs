namespace AccountManagement.Business.AccountDomain.Responses
{
    public class AccountResponse
    {
        public string Id { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public AccountResponse(string id, string email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}