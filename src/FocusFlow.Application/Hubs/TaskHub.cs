using Microsoft.AspNetCore.SignalR;

namespace FocusFlow.Application.Hubs;

public class TaskHub : Hub
{
    /// <summary>
    /// Broadcasts a notification to all connected clients when a task is created
    /// </summary>
    /// <param name="taskId">The ID of the newly created task</param>
    public async Task NotifyTaskCreated(long taskId)
    {
        await Clients.All.SendAsync("TaskCreated", taskId);
    }

    /// <summary>
    /// Broadcasts a notification to all connected clients when a task is updated
    /// </summary>
    /// <param name="taskId">The ID of the updated task</param>
    public async Task NotifyTaskUpdated(long taskId)
    {
        await Clients.All.SendAsync("TaskUpdated", taskId);
    }

    /// <summary>
    /// Broadcasts a notification to all connected clients when a task is deleted
    /// </summary>
    /// <param name="taskId">The ID of the deleted task</param>
    public async Task NotifyTaskDeleted(long taskId)
    {
        await Clients.All.SendAsync("TaskDeleted", taskId);
    }
}
