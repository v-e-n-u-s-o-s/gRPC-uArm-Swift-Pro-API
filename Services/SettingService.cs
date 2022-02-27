using Grpc.Core;
using gRPC_uArm_API.Models;

namespace gRPC_uArm_API.Services
{
    public class SettingService : Protos.Setting.SettingBase
    {
        private readonly ILogger<SettingService> logger;
        public SettingService(ILogger<SettingService> logger)
        {
            this.logger = logger;
        }
    }
}
