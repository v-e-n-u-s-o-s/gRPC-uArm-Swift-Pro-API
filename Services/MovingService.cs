using Grpc.Core;
using gRPC_uArm_API.Models;

namespace gRPC_uArm_API.Services
{
    public class MovingService : Protos.Moving.MovingBase
    {
        private readonly ILogger<MovingService> logger;
        public MovingService(ILogger<MovingService> logger)
        {
            this.logger = logger;
        }

        //----------------------[ G0 ]----------------------

        public override async Task G0(IAsyncStreamReader<Protos.G0Request> requestStream, IServerStreamWriter<Protos.G0Response> responseStream, ServerCallContext context)
        {
            await Task.WhenAll(G0RequestHandler(requestStream, context), G0ResponseHandler(responseStream, context));
        }
        private async Task G0RequestHandler(IAsyncStreamReader<Protos.G0Request> requestStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested && await requestStream.MoveNext())
            {
                var request = requestStream.Current;
                logger.LogInformation(request.ToString());
                Commands.Send($"G0 X{request.X} Y{request.Y} Z{request.Z} F{request.F}\n");
            }
        }
        private static async Task G0ResponseHandler(IServerStreamWriter<Protos.G0Response> responseStream, ServerCallContext context)
        {
            if (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new Protos.G0Response
                {
                    Status = Commands.Recive().Result
                });
            }
        }
    }
}
