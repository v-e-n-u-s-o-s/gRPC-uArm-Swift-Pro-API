using Grpc.Core;
using gRPC_uArm_API.Models;

namespace gRPC_uArm_API.Services
{
    public class QueryingService : Protos.Querying.QueryingBase
    {
        private readonly ILogger<QueryingService> logger;
        public QueryingService(ILogger<QueryingService> logger)
        {
            this.logger = logger;
        }
    }
}
