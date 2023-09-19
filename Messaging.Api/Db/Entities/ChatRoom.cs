using Messaging.Api.Db.Enums;

namespace Messaging.Api.Db.Entities;

public class ChatRoom
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public RoomType Type { get; set; }
}