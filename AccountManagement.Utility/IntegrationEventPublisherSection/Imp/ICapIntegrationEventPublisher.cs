using DotNetCore.CAP;

namespace AccountManagement.Utility.IntegrationEventPublisherSection.Imp
{
    public interface ICapIntegrationEventPublisher : IIntegrationEventPublisher,
                                                     ICapPublisher
    {
    }
}