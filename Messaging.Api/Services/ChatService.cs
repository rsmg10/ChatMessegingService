using Messaging.Api.Db;
using Messaging.Api.Domain.Entities;
using Messaging.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Api.Services;

public  class ChatService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ChatDb _dbContext;

    public ChatService(IHubContext<ChatHub> hubContext, ChatDb dbContext)
    {
        _hubContext = hubContext;
        _dbContext = dbContext;
    }


    public async Task<List<Room>> GetUserRooms(string? email)
    {
        var user = await _dbContext.Users.Include(u=> u.Rooms).FirstOrDefaultAsync(user => user.Username.Equals(email));

        return user?.Rooms ?? new List<Room>();
    }
}

