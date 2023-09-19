using Messaging.Api.Db;
using Messaging.Api.Db.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Messaging.Api.Services;

public abstract class ChatService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ChatDb _dbContext;

    public ChatService(IHubContext<ChatHub> hubContext, ChatDb dbContext)
    {
        _hubContext = hubContext;
        _dbContext = dbContext;
    }

    public async Task SendMessage(string userName, string messageContent)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == userName);
        if (user == null)
        {
            user = new User { Name = userName };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        var message = new Message
        {
            Content = messageContent,
            Timestamp = DateTime.Now,
            UserId = user.Id
        };

        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}

public class User
{
    public string Name { get; set; }
}