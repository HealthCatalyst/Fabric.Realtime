namespace Fabric.Realtime.Handlers
{
    using System.Threading.Tasks;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.Domain.Stores;
    using Fabric.Realtime.EventBus.Models;
    using Fabric.Realtime.EventBus.Services;
    using Fabric.Realtime.Transformers;

    using Newtonsoft.Json;

    public class InterfaceEngineEventHandler : IInterfaceEngineEventHandler
    {
        private readonly ExternalApplicationQueueService _externalApplicationQueueService;

        private readonly MessageTypeSubscriberService _messageTypeSubscriberService;

        private readonly RealtimeContext _realtimeContext;

        private readonly IInterfaceEngineMessageTransformer _transformer;

        public InterfaceEngineEventHandler(
            IInterfaceEngineMessageTransformer transformer,
            RealtimeContext context,
            MessageTypeSubscriberService messageTypeSubscriberService,
            ExternalApplicationQueueService queueService)
        {
            this._transformer = transformer;
            this._realtimeContext = context;
            this._messageTypeSubscriberService = messageTypeSubscriberService;
            this._externalApplicationQueueService = queueService;
        }

        public Task HandleMessage(string rawMessage)
        {
            // 1. Deserialize
            InterfaceEngineMessage interfaceEngineMessage =
                JsonConvert.DeserializeObject<HL7InterfaceEngineMessage>(rawMessage);

            // 2. Transform
            var message = this._transformer.Transform(interfaceEngineMessage);

            // 3. Persist
            if (message.Protocol.Equals(MessageProtocol.HL7))
            {
                this._realtimeContext.HL7Messages.Add((HL7Message)message);
                this._realtimeContext.SaveChanges();
            }

            // 4. Queue for routing
            var subscriptions = this._messageTypeSubscriberService.GetSubscriptions(((HL7Message)message).MessageType);
            foreach (var subscription in subscriptions)
                this._externalApplicationQueueService.ForwardMessage(subscription.RoutingKey, (HL7Message)message);

            // 5. Return task
            return Task.CompletedTask;
        }
    }
}