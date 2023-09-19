using Messaging.Api.Db.Enums;

namespace Messaging.Api.Domain.Dto;

public class SendMessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public MessageType Type { get; set; }
    public long Sequence { get; set; }
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }
    public IFormFile? File { get; set; }
}
// or the client could upload the file independently and send an Id