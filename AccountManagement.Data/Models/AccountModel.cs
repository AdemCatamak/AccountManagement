using MassTransit;

namespace AccountManagement.Data.Models
{
    public class AccountModel
    {
        public string Id { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public AccountModel(string email, string firstName, string lastName)
            : this(NewId.Next().ToString(), email, firstName, lastName)
        {
        }

        public AccountModel(string id, string email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}