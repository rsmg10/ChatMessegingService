using Messaging.Api.Domain.Entities;

namespace Messaging.Api.Domain.Dto;

public class RoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<UserDto> Users { get; set; } = new List<UserDto>();
    public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
}