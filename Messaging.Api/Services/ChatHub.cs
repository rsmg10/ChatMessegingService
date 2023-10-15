using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messaging.Api.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Messaging.Api.Services;

public interface IToClient
{
    // void CallMe();
}

public interface IToServer
{
    // void CallMe();
}

public class EmailBasedUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Email)?.Value!;
    }
}

public class ChatHub : Hub
{
    
    public async Task SendToGroup(string roomName, string message)
    {
        await Clients.Groups(roomName).SendAsync("ReceiveMessage", message, "___");
    }

    public override Task OnConnectedAsync()
    {
        // var groupNames = GetGroupNames();
        // foreach (var groupName in groupNames)
        // {
        //     Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        // }
        
        return base.OnConnectedAsync();
    }
    
    public Task SendMessageToAll(string message)
    {
        return Clients.All.SendAsync("ReceiveMessage", message);
    }

    public Task SendMessageToCaller(string message)
    {
        return Clients.Caller.SendAsync("ReceiveMessage", message);
    }

    public Task SendMessageToUser(string connectionId, string message)
    {
        return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
    }

    public Task JoinGroup(string group)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    public override async Task OnDisconnectedAsync(Exception ex)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(ex);
    }

    public async Task JoinRoom(string roomName, string message)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public async Task LeaveRoom(string roomName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }

    public async Task SendMessage(string roomName)
    {
        try
        {
            var claims = GetClaims();
            
            // Check for permissions (example)
            var hasPermission = claims.Any(c => c.Type == "email");

            if (hasPermission)
            {
                // Process the request
                await Clients.All.SendAsync("ReceiveMessage", "success");
            }
            else
            {
                // Handle unauthorized access
                await Clients.Caller.SendAsync("ErrorMessage", "You do not have permission to send messages.");
            }
      
            await Clients.Caller.SendAsync("ReceiveMessage", Context.ConnectionId, Context.User.Claims);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
     }

    private IEnumerable<Claim> GetClaims()
    {
        var accessToken = Context.GetHttpContext().Request.Query["access_token"];

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = "issuer", // Replace with your actual issuer
            ValidAudience = "audience", // Replace with your actual audience
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        "RadwanLevelSuperSecretKeyPleaseDontHackMessssssssssssdfSDFSDfsDFSDFSDf")) // Replace with your actual security key
        };

        var handler = new JwtSecurityTokenHandler();
        var claimsPrincipal = handler.ValidateToken(accessToken, validationParameters, out var _);

        return claimsPrincipal.Claims;
    }

    // public async Task SendMessage(string roomName, string user, string message)
    // {
    //     await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
    // }
    // Method to process the incoming request



    private string[] GetGroupNames()
    {
        return new[] { "room1", "room2" };
    }

    ////////////////////
    ///
    // public override async Task OnConnectedAsync()
    // {
    //     // Retrieve the user's permissions based on their authentication
    //     var userPermissions = await RetrieveUserPermissions(Context.User.Identity.Name);
    //
    //     // Store the user's permissions in the Context
    //     Context.Items["Permissions"] = userPermissions;
    //
    //     
    //     // if has permission 
    //         await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
    //     await base.OnConnectedAsync();
    // }
    // [GroupAuthorization("AdminGroup")]
    // public async Task SendMessageToGroup(string groupName, string message)
    // {
    //     // Check if the user has the necessary permissions to send a message to the group
    //     var userPermissions = (List<string>)Context.Items["Permissions"];
    //     var requiredPermissions = GetRequiredPermissionsForGroup(groupName);
    //
    //     if (userPermissions.Intersect(requiredPermissions).Any())
    //     {
    //         // User has the necessary permissions, send the message to the group
    //         await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
    //     }
    //     else
    //     {
    //         // User does not have the necessary permissions, handle accordingly
    //         await Clients.Caller.SendAsync("ErrorMessage", "You are not authorized to send messages to this group.");
    //     }
    //    // return Clients.Group(group).SendAsync("ReceiveMessage", message);
    // }
    
    public async Task ProcessRequest(string message)
    {
        // Get the access token from the incoming request
        var accessToken = Context.GetHttpContext().Request.Query["access_token"];

        // Decode and verify the access token
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);

        // Get the claims from the token
        var claims = token.Claims;

        // Check for permissions (example)
        var hasPermission = claims.Any(c => c.Type == "permission" && c.Value == "canSendMessage");

        if (hasPermission)
        {
            // Process the request
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        else
        {
            // Handle unauthorized access
            await Clients.Caller.SendAsync("ErrorMessage", "You do not have permission to send messages.");
        }
    }

    
    
}