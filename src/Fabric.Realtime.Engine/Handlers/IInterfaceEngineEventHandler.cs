namespace Fabric.Realtime.Engine.Handlers
{
    using System.Threading.Tasks;

    public interface IInterfaceEngineEventHandler
    {
        Task HandleMessage(string message);
    }
}