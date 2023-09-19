using Microsoft.AspNetCore.SignalR;

namespace Messaging.Api.Services;

public class ChatHub : Hub
{
    public async Task SendMessage(string userName, string messageContent)
    {
        var chatService = Context.GetHttpContext().RequestServices.GetService<ChatService>();
        await chatService.SendMessage(userName, messageContent);
    }
}