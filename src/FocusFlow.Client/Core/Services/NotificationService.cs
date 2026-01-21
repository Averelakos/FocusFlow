namespace FocusFlow.Client.Core.Services;

public class NotificationService
{
    public event Action<string, NotificationType>? OnNotification;

    public void ShowSuccess(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Success);
    }

    public void ShowError(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Error);
    }

    public void ShowInfo(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Info);
    }

    public void ShowWarning(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Warning);
    }
}

public enum NotificationType
{
    Success,
    Error,
    Info,
    Warning
}
