namespace Messaging.Api.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual List<User> Users { get; set; } = new List<User>();
    public virtual List<Message> Messages { get; set; } = new List<Message>();
}