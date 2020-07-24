using AccountManagement.Business.AccountDomain.Responses;
using AccountManagement.Utility.IntegrationEventPublisherSection;

namespace AccountManagement.Business.AccountDomain.Events
{
    public class AccountCreatedEvent : IIntegrationEvent
    {
        public AccountResponse AccountResponse { get; private set; }

        public AccountCreatedEvent(AccountResponse accountResponse)
        {
            AccountResponse = accountResponse;
        }

        public override string ToString()
        {
            return $"{AccountResponse.Id} - {AccountResponse.Email}";
        }
    }
}