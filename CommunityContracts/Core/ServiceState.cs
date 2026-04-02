
using static CommunityContracts.Core.NPCServiceMenu;

namespace CommunityContracts.Core
{
    public static class ServiceState
    {
        public static readonly Dictionary<ServiceId, bool> IsCollectingService = new();
        public static readonly Dictionary<string, bool> IsNPCCollecting = new();
    }
}
