namespace Fabric.Realtime.Handlers
{
    using System.Threading.Tasks;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.EventBus.Models;
    using Fabric.Realtime.Transformers;

    using Newtonsoft.Json;

    public class InterfaceEngineEventHandler : IInterfaceEngineEventHandler
    {
        IInterfaceEngineMessageTransformer _transformer;

        public InterfaceEngineEventHandler(IInterfaceEngineMessageTransformer transformer)
        {
            this._transformer = transformer;
        }

        public Task HandleMessage(string rawMessage)
        {
            // 1. Deserialize
            InterfaceEngineMessage interfaceEngineMessage =
                JsonConvert.DeserializeObject<InterfaceEngineMessage>(rawMessage);

            // 2. Transform
            Message message = this._transformer.Transform(interfaceEngineMessage);

            // 3. Persist

            // 4. Queue for routing

            // 5. Return task
            return Task.CompletedTask;
        }
    }
}