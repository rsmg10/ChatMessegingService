namespace Messaging.Api.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsOnline { get; set; }
    public virtual List<Room> Rooms { get; set; }
    public virtual List<Message> Messages { get; set; }
}