using Messaging.Api.Db;
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
}

