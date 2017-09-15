using Catalyst.Logging.Abstractions;
using DryIoc;
using Fabric.Realtime.Core;
using Fabric.Realtime.Engine;
using Fabric.Realtime.Engine.Record;
using Fabric.Realtime.Engine.Replay;

namespace Fabric.Realtime.ConsoleApp
{
    public static class Bootstrapper
    {
        public static IContainer Initialize()
        {
            var container = new Container();
            container.Register<IBackgroundWorker, MessageReceiveWorker>();
            container.Register<IBackgroundWorker, MessageReplayWorker>();
            return container;
        }
    }
}
