using System.Threading;
using System.Threading.Tasks;

namespace AccountManagement.Utility.IntegrationEventPublisherSection
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : IIntegrationEvent;
    }
}