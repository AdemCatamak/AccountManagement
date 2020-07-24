using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace AccountManagement.Utility.IntegrationEventPublisherSection.Imp
{
    public class CapIntegrationEventPublisher : ICapIntegrationEventPublisher
    {
        private readonly ICapPublisher _capPublisher;

        public CapIntegrationEventPublisher(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Publish<T>(T message, CancellationToken cancellationToken = default)
            where T : IIntegrationEvent
        {
            string queueName = message.GetType().Name;
            await _capPublisher.PublishAsync(queueName, message, cancellationToken: cancellationToken);
        }

        public Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = new CancellationToken())
        {
            return _capPublisher.PublishAsync(name, contentObj, callbackName, cancellationToken);
        }

        public Task PublishAsync<T>(string name, T contentObj, IDictionary<string, string> headers, CancellationToken cancellationToken = new CancellationToken())
        {
            return _capPublisher.PublishAsync(name, contentObj, headers, cancellationToken);
        }

        public void Publish<T>(string name, T contentObj, string callbackName = null)
        {
            _capPublisher.Publish(name, contentObj, callbackName);
        }

        public void Publish<T>(string name, T contentObj, IDictionary<string, string> headers)
        {
            _capPublisher.Publish(name, contentObj, headers);
        }

        public IServiceProvider ServiceProvider => _capPublisher.ServiceProvider;
        public AsyncLocal<ICapTransaction> Transaction => _capPublisher.Transaction;
    }
}