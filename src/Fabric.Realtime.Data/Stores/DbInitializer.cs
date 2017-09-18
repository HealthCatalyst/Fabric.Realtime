namespace Fabric.Realtime.Data.Stores
{
    public class DbInitializer
    {
        public static void Initialize(RealtimeContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}