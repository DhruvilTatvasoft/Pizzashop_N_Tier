using Microsoft.AspNetCore.SignalR;

public class KotHub : Hub
{
    // public override async Task OnConnectedAsync()
    // {
    //     await Clients.All.SendAsync("ReceiveMessage", $"Received the message: Another connection has been added.");
    // }

    // public async Task SendMessage(string message)
    // {
    //     await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", $"Received the message: {message}");
    // }
}