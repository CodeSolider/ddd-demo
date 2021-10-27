using EbayPlatform.Domain.Core.Abstractions;
using System.Runtime.CompilerServices;

namespace EbayPlatform.Infrastructure.Core.Engines
{
    public class EngineContext
    {
        private static IEngine engine;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(IEngine _engine)
        {
            if (engine == null)
                engine = _engine;
            return engine;
        }

        public static IEngine Current => engine;
    }
}
