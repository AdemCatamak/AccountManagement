using AccountManagement.Business.AccountDomain.Responses;
using AccountManagement.Data.Models;

namespace AccountManagement.Business.AccountDomain.Mappings
{
    public static class AccountResponseMappings
    {
        public static AccountResponse ToAccountResponse(this AccountModel accountModel)
        {
            return new AccountResponse(accountModel.Id, accountModel.Email, accountModel.FirstName, accountModel.LastName);
        }
    }
}