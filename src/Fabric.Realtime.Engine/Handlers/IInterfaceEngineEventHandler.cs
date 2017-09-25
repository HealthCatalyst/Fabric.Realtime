namespace Fabric.Realtime.Engine.Handlers
{
    using System.Threading.Tasks;

    /// <summary>
    /// The interface engine callback interface.
    /// </summary>
    public interface IInterfaceEngineEventHandler
    {
        bool HandleMessage(string message);
    }
}