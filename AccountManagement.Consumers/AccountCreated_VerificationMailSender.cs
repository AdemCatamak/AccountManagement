using System;
using System.Threading.Tasks;
using AccountManagement.Business.AccountDomain.Events;
using DotNetCore.CAP;

namespace AccountManagement.Consumers
{
    public class AccountCreated_VerificationMailSender : AccountCreatedEventHandler,
                                                         ICapSubscribe
    {
        [CapSubscribe(nameof(AccountCreatedEvent), Group = nameof(AccountCreated_VerificationMailSender))]
        public override Task Handle(AccountCreatedEvent accountCreatedEvent)
        {
            Console.WriteLine($"{accountCreatedEvent} is received.{Environment.NewLine}" +
                              $"Email will be sent here");
            return Task.CompletedTask;
        }
    }
}