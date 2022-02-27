using gRPC_uArm_API.Services;
using gRPC_uArm_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var setting = builder.Configuration.GetSection("Options").Get<Setting>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MovingService>();
app.MapGrpcService<SettingService>();
app.MapGrpcService<QueryingService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

new Commands(setting);

app.Run();