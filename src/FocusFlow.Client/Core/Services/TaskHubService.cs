using Microsoft.AspNetCore.SignalR.Client;

namespace FocusFlow.Client.Core.Services;

public class TaskHubService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;

    public event Func<long, Task>? TaskCreated;
    public event Func<long, Task>? TaskUpdated;
    public event Func<long, Task>? TaskDeleted;

    public TaskHubService()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5094/taskhub", options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    // If you need to add authentication token, add it here
                    return null;
                };
            })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<long>("TaskCreated", (taskId) =>
        {
            return TaskCreated?.Invoke(taskId) ?? Task.CompletedTask;
        });

        _hubConnection.On<long>("TaskUpdated", (taskId) =>
        {
            return TaskUpdated?.Invoke(taskId) ?? Task.CompletedTask;
        });

        _hubConnection.On<long>("TaskDeleted", (taskId) =>
        {
            return TaskDeleted?.Invoke(taskId) ?? Task.CompletedTask;
        });
    }

    public async Task StartAsync()
    {
        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR Connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR Connection Error: {ex.Message}");
            }
        }
    }

    public async Task StopAsync()
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.StopAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
