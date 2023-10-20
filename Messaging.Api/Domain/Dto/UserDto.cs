using Messaging.Api.Domain.Entities;

namespace Messaging.Api.Domain.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public bool IsOnline { get; set; }
    public List<RoomDto> Rooms { get; set; }
    public List<MessageDto> Messages { get; set; }
}