using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using StructureGrpc;
namespace Grpc_Client;

public class StructureGrpcClient : IStructureGrpcClient, IDisposable
{
    private readonly StructureServiceDefinition.StructureServiceDefinitionClient _client;
    private readonly GrpcChannel _channel;
    private readonly ILogger<StructureGrpcClient> _logger;

    public StructureGrpcClient(string grpcServiceUrl, ILogger<StructureGrpcClient> logger)
    {
        _logger = logger;

        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var options = new GrpcChannelOptions
        {
            HttpHandler = httpHandler
        };

        _channel = GrpcChannel.ForAddress(grpcServiceUrl, options);
        _client = new StructureServiceDefinition.StructureServiceDefinitionClient(_channel);
    }

    public async Task<EmployeeResponse> GetEmployees(GlobalRequest request)
    {
        try
        {
            // Create a CancellationTokenSource with a timeout of 1 minute
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            // Initiate the gRPC call with the cancellation token
            var grpcCallTask = _client.GetEmployeesAsync(request, cancellationToken: cts.Token).ResponseAsync;

            // Wait for the gRPC task to complete or for the timeout
            var completedTask = await Task.WhenAny(grpcCallTask, Task.Delay(Timeout.Infinite, cts.Token));

            // Check if the gRPC call task completed
            if (completedTask == grpcCallTask)
            {
                // gRPC call completed successfully
                return await grpcCallTask;
            }
            else
            {
                // Timeout occurred
                throw new TimeoutException("The gRPC call timed out after 1 minute.");
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            // Handle gRPC timeout scenario
            Console.WriteLine("The structure gRPC request timed out.");
            return null; // Or handle appropriately
        }
        catch (TimeoutException ex)
        {
            // Handle client-side enforced timeout
            Console.WriteLine(ex.Message);
            return null; // Or handle appropriately
        }
        catch (OperationCanceledException)
        {
            // Handle operation cancellation (e.g., due to timeout)
            Console.WriteLine("The structure gRPC call was canceled.");
            return null; // Or handle appropriately
        }
    }

    public async Task<JobsResponse> GetJobs(GlobalRequest request)
    {
        try
        {
            // Create a CancellationTokenSource with a timeout of 1 minute
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            // Initiate the gRPC call with the cancellation token
            var grpcCallTask = _client.GetJobsAsync(request, cancellationToken: cts.Token).ResponseAsync;

            // Wait for the gRPC task to complete or for the timeout
            var completedTask = await Task.WhenAny(grpcCallTask, Task.Delay(Timeout.Infinite, cts.Token));

            // Check if the gRPC call task completed
            if (completedTask == grpcCallTask)
            {
                // gRPC call completed successfully
                return await grpcCallTask;
            }
            else
            {
                // Timeout occurred
                throw new TimeoutException("The gRPC call timed out after 1 minute.");
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            // Handle gRPC timeout scenario
            Console.WriteLine("The structure gRPC request timed out.");
            return null; // Or handle appropriately
        }
        catch (TimeoutException ex)
        {
            // Handle client-side enforced timeout
            Console.WriteLine(ex.Message);
            return null; // Or handle appropriately
        }
        catch (OperationCanceledException)
        {
            // Handle operation cancellation (e.g., due to timeout)
            Console.WriteLine("The structure gRPC call was canceled.");
            return null; // Or handle appropriately
        }
    }

    public void Dispose()
    {
        _channel?.ShutdownAsync().Wait();
    }
}


public interface IStructureGrpcClient
{
    Task<EmployeeResponse> GetEmployees(GlobalRequest request);
    Task<JobsResponse> GetJobs(GlobalRequest request);

}
