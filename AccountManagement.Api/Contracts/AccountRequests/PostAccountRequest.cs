namespace AccountManagement.Api.Contracts.AccountRequests
{
    public class PostAccountRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}