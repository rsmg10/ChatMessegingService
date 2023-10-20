namespace Messaging.Api.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public Guid RoomId { get; set; }
    public virtual  Room Room { get; set; } = null!;
}