using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fabric.Realtime.Domain.Stores
{
    using Microsoft.AspNetCore.Builder;

    public class DbInitializer
    {
        public static void Initialize(RealtimeContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
