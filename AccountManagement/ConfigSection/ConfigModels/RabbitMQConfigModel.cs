using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagement.ConfigSection.ConfigModels
{
    public class RabbitMQConfigModel
    {
        public List<RabbitMQOption> RabbitMQOptions { get; set; }
        public int SelectedIndex { get; set; }

        public RabbitMQOption SelectedRabbitMQOption()
        {
            if (RabbitMQOptions == null)
                throw new ArgumentNullException(nameof(RabbitMQOptions));

            if (!RabbitMQOptions.Any())
                throw new ArgumentException($"{nameof(RabbitMQOptions)} is empty");

            RabbitMQOption selectedRabbitMQOption = RabbitMQOptions.FirstOrDefault(o => o.Index == SelectedIndex);

            if (selectedRabbitMQOption == null)
                throw new ArgumentOutOfRangeException($"{nameof(RabbitMQOption)} could not found. {nameof(SelectedIndex)} : {SelectedIndex}");

            return selectedRabbitMQOption;
        }
    }

    public class RabbitMQOption
    {
        public int Index { get; set; }
        public MessageBrokerTypes BrokerType { get; set; }
        public string BrokerName { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public enum MessageBrokerTypes
    {
        RabbitMq = 1,
    }
}