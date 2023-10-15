namespace Messaging.Api.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; } = new List<User>();
    public List<Message> Messages { get; set; } = new List<Message>();
}