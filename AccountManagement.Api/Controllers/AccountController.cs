using System.Net;
using System.Threading.Tasks;
using AccountManagement.Api.Contracts.AccountRequests;
using AccountManagement.Business.AccountDomain;
using AccountManagement.Business.AccountDomain.Requests;
using AccountManagement.Business.AccountDomain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Api.Controllers
{
    [Route("accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("")]
        public async Task<IActionResult> PostAccount([FromBody] PostAccountRequest postAccountRequest)
        {
            var createAccountCommand = new CreateAccountCommand
                (
                 postAccountRequest?.Email,
                 postAccountRequest?.FirstName,
                 postAccountRequest?.LastName
                );

            AccountResponse accountResponse = await _accountService.CreateAccountAsync(createAccountCommand);

            return StatusCode((int) HttpStatusCode.Created, accountResponse);
        }
    }
}