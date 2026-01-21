using Microsoft.AspNetCore.SignalR.Client;

namespace FocusFlow.Client.Core.Services;

    private readonly HubConnection _hubConnection;

    /// <summary>Event raised when a task is created</summary>
    public event Func<long, Task>? TaskCreated;
    /// <summary>Event raised when a task is updated</summary>
    public event Func<long, Task>? TaskUpdated;
    /// <summary>Event raised when a task is deleted</summary>
    public event Func<long, Task>? TaskDeleted;

    /// <summary>
    /// Initializes the SignalR connection and registers event handlers
    /// </summary>
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

    /// <summary>
    /// Starts the SignalR connection if disconnected
    /// </summary>
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

    /// <summary>
    /// Stops the SignalR connection if connected
    /// </summary>
    public async Task StopAsync()
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.StopAsync();
        }
    }

    /// <summary>
    /// Disposes the SignalR connection
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
