using Microsoft.AspNetCore.SignalR;

namespace FocusFlow.Application.Hubs;

public class TaskHub : Hub
{
    public async Task NotifyTaskCreated(long taskId)
    {
        await Clients.All.SendAsync("TaskCreated", taskId);
    }

    public async Task NotifyTaskUpdated(long taskId)
    {
        await Clients.All.SendAsync("TaskUpdated", taskId);
    }

    public async Task NotifyTaskDeleted(long taskId)
    {
        await Clients.All.SendAsync("TaskDeleted", taskId);
    }
}
