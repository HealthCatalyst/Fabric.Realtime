namespace Fabric.Realtime.Handlers
{
    using System.Threading.Tasks;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.EventBus.Models;
    using Fabric.Realtime.Transformers;

    using Newtonsoft.Json;
    using Fabric.Realtime.Domain.Stores;

    public class InterfaceEngineEventHandler : IInterfaceEngineEventHandler
    {
        IInterfaceEngineMessageTransformer _transformer;
        private readonly RealtimeContext _realtimeContext;

        public InterfaceEngineEventHandler(IInterfaceEngineMessageTransformer transformer, RealtimeContext context)
        {
            this._transformer = transformer;
            this._realtimeContext = context;
        }

        public Task HandleMessage(string rawMessage)
        {
            // 1. Deserialize
            InterfaceEngineMessage interfaceEngineMessage =
                JsonConvert.DeserializeObject<HL7InterfaceEngineMessage>(rawMessage);

            // 2. Transform
            Message message = this._transformer.Transform(interfaceEngineMessage);

            // 3. Persist
            if (message.Protocol.Equals(MessageProtocol.HL7))
            {
                this._realtimeContext.HL7Messages.Add((HL7Message)message);
                this._realtimeContext.SaveChanges();
            }

            // 4. Queue for routing

            // 5. Return task
            return Task.CompletedTask;
        }
    }
}