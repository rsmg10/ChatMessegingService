namespace Messaging.Api.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class GroupAuthorizationAttribute : Attribute
{
    public string GroupName { get; }

    public GroupAuthorizationAttribute(string groupName)
    {
        GroupName = groupName;
    }
}