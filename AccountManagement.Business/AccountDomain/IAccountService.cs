using System.Threading.Tasks;
using AccountManagement.Business.AccountDomain.Requests;
using AccountManagement.Business.AccountDomain.Responses;

namespace AccountManagement.Business.AccountDomain
{
    public interface IAccountService
    {
        Task<AccountResponse> CreateAccountAsync(CreateAccountCommand createAccountCommand);
    }
}