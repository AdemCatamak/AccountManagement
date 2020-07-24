using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Business.AccountDomain.Events;
using AccountManagement.Business.AccountDomain.Mappings;
using AccountManagement.Business.AccountDomain.Requests;
using AccountManagement.Business.AccountDomain.Responses;
using AccountManagement.Data;
using AccountManagement.Data.Models;
using AccountManagement.Exceptions;
using AccountManagement.Exceptions.Imp;
using AccountManagement.Utility.IntegrationEventPublisherSection.Imp;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountManagement.Business.AccountDomain
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _dataContext;
        private readonly ICapIntegrationEventPublisher _integrationEventPublisher;

        public AccountService(DataContext dataContext, ICapIntegrationEventPublisher integrationEventPublisher)
        {
            _dataContext = dataContext;
            _integrationEventPublisher = integrationEventPublisher;
        }

        public async Task<AccountResponse> CreateAccountAsync(CreateAccountCommand createAccountCommand)
        {
            if (createAccountCommand == null) throw new RequestNullException();

            AccountResponse accountResponse;

            bool alreadyExist = _dataContext.AccountModels.Any(m => m.Email == createAccountCommand.Email);
            if (alreadyExist) throw new EmailAlreadyRegisteredException(createAccountCommand.Email);
            
            await using (IDbContextTransaction transaction = _dataContext.Database.BeginTransaction(_integrationEventPublisher, autoCommit: true))
            {
                var accountModel = new AccountModel(createAccountCommand.Email, createAccountCommand.FirstName, createAccountCommand.LastName);
                await _dataContext.AccountModels.AddAsync(accountModel);
                await _dataContext.SaveChangesAsync();

                accountResponse = accountModel.ToAccountResponse();
                var accountCreatedEvent = new AccountCreatedEvent(accountResponse);

                await _integrationEventPublisher.Publish(accountCreatedEvent);
            }

            return accountResponse;
        }
    }
}