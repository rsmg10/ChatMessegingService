using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messaging.Api.Db;
using Messaging.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Messaging.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RoomsController : ControllerBase
{
    private readonly ChatDb _db;

    public RoomsController(ChatDb db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(string name)
    {
        var nameTaken = await _db.Rooms.AnyAsync(x => x.Name == name);

        if (nameTaken) return BadRequest("this name is already taken");

        var room = new Room()
        {
            Name = name,
        };

        await _db.Rooms.AddAsync(room);

        var saved = await _db.SaveChangesAsync() > 0;

        return saved ? Ok("room created") : BadRequest("could not save");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _db.Rooms.Include(x=> x.Users).ToListAsync();
        return Ok(rooms);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsersInRoom(string roomName)
    {
        var room = await _db.Rooms.Include(x => x.Users).FirstOrDefaultAsync(x=> x.Name == roomName);
        if (room == null) return NotFound("room not found");

        return Ok(room.Users);
    }

    
}