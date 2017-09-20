namespace Fabric.Realtime.Engine.Transformers
{
    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Engine.EventBus.Models;

    /// <summary>
    /// The InterfaceEngineMessageTransformer interface.
    /// </summary>
    public interface IInterfaceEngineMessageTransformer
    {
        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="IMessage"/>.
        /// </returns>
        IMessage Transform(InterfaceEngineMessage message);
    }
}