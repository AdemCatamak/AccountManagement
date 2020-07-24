using System.Threading.Tasks;
using AccountManagement.Business.AccountDomain.Events;

namespace AccountManagement.Consumers
{
    public abstract class AccountCreatedEventHandler
    {
        public abstract Task Handle(AccountCreatedEvent accountCreatedEvent);
    }
}