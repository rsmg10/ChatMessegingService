using Messaging.Api.Domain.Entities;

namespace Messaging.Api.Domain.Dto;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserDto User { get; set; } = null!;
    public Guid RoomId { get; set; }
    public RoomDto Room { get; set; } = null!;
}