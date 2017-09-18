namespace Fabric.Realtime.Engine.Transformers
{
    using Fabric.Realtime.Data.Models;
    using Fabric.Realtime.Engine.EventBus.Models;

    public interface IInterfaceEngineMessageTransformer
    {
        IMessage Transform(InterfaceEngineMessage message);
    }
}