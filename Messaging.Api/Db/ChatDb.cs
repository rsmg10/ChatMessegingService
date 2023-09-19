using Messaging.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Api.Db;

public class ChatDb : DbContext
{
    
    private DbSet<User> Users { get; set; }
}