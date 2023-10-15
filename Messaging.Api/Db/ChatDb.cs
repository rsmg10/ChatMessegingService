using Messaging.Api.Domain.Entities;
using Messaging.Api.Domain.Entities.JointEntities;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Api.Db;

public class ChatDb : DbContext
{
    public ChatDb(DbContextOptions<ChatDb> context): base(context)
    {
    }
    
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

            optionsBuilder.UseSqlServer(
                "Data Source=.;Database=chatDb;Trusted_Connection=true;TrustServerCertificate=True;");
        }
        base.OnConfiguring(optionsBuilder);
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Rooms)
            .UsingEntity<UserRooms>();
        
        modelBuilder.Entity<Room>()
            .HasMany(e => e.Messages)
            .WithOne(e => e.Room);
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.Messages)
            .WithOne(e => e.User);
        
    }
}
