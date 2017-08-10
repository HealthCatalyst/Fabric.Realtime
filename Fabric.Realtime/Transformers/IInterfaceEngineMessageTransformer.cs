namespace Fabric.Realtime.Transformers
{
    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.EventBus.Models;

    public interface IInterfaceEngineMessageTransformer
    {
        Message Transform(InterfaceEngineMessage message);
    }
}