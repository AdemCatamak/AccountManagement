using System;
using System.Threading.Tasks;
using AccountManagement.Business.AccountDomain.Events;
using DotNetCore.CAP;

namespace AccountManagement.Consumers
{
    public class AccountCreated_AnotherOperation : AccountCreatedEventHandler,
                                                   ICapSubscribe
    {
        [CapSubscribe(nameof(AccountCreatedEvent), Group = nameof(AccountCreated_AnotherOperation))]
        public override Task Handle(AccountCreatedEvent accountCreatedEvent)
        {
            Console.WriteLine($"{accountCreatedEvent} is received.{Environment.NewLine}" +
                              $"Another operation is executed");
            return Task.CompletedTask;
        }
    }
}