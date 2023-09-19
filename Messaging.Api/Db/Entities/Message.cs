using Messaging.Api.Db.Enums;

namespace Messaging.Api.Db.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public MessageType Type { get; set; }
    public long Sequence { get; set; }
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime Timestamp { get; set; }
}