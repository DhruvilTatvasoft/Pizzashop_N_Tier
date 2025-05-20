using Microsoft.AspNetCore.SignalR;

public interface INotificationService
{
    Task    SendMessageToAll(string message);
    Task SendMessageToUser(string userId, string message);
}

public class NotificationService : INotificationService
{
    private readonly IHubContext<KotHub> _hubContext;

    public NotificationService(IHubContext<KotHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMessageToAll(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
    public async Task SendMessageToUser(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
    }
}
