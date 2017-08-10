namespace Fabric.Realtime.Handlers
{
    using System.Threading.Tasks;

    public interface IInterfaceEngineEventHandler
    {
        Task HandleMessage(string message);
    }
}