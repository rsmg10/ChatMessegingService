using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using MapsterMapper;
using Messaging.Api.Db;
using Messaging.Api.Domain.Dto;
using Messaging.Api.Domain.Entities;
using Messaging.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Messaging.Api.Controllers;

[ApiController]
// [Produces("application/xml")]
[Route("[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly ChatDb _db;
    private readonly IMapper _mapper;


    public UsersController(ChatDb db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    [HttpGet]
    public string GetAccessTokenWithClaims(string email)
    {
        var claims = Enumerable.Empty<Claim>();
        claims = claims.Append(new Claim("email",email));
        
        // Create the token descriptor with claims
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RadwanLevelSuperSecretKeyPleaseDontHackMessssssssssssdfSDFSDfsDFSDFSDf"));
        var token = new JwtSecurityToken(
            "issuer123",
            audience: "audience",
            expires: DateTime.UtcNow.AddDays(123),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    [HttpGet]
    public User GetUser(User user)
    {
        return new User();
    }
    
    [HttpGet]
    public async Task<IActionResult> SignUp(string name, string username, string password )
    {
        if (await _db.Users.AnyAsync(x => x.Username == username)) BadRequest("username already exists");
        await _db.Users.AddAsync(new User()
        {
            Username = username,
            Name = name,
            Password = password,

        });
        var saved = await _db.SaveChangesAsync() > 0;

        return saved ? Ok("succesffly added user") : BadRequest("something went wrong");
    }
    
        [HttpGet]
        public async Task<IActionResult> Signin(string username, string password )
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            if (user == null) return NotFound("user or password is not correct");

            var token = GetAccessTokenWithClaims(user.Username);
            
            return Ok(token);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _db.Users.Include(x=> x.Rooms).ProjectToType<UserDto>(_mapper.Config)
                .ToListAsync();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRoom(string username, string roomName)
        {
            var user = await _db.Users.Include(x=> x.Rooms).FirstOrDefaultAsync(x=> x.Username == username);
            if (user == null) return NotFound("user not found");
            
            var room = await _db.Rooms.Include(x => x.Users).FirstOrDefaultAsync(x=> x.Name == roomName);
            if (room == null) return NotFound("room not found");

            if (room.Users.Contains(user)) return Ok("user already in room");
            
            room.Users.Add(user);
            
            var saved = await _db.SaveChangesAsync() > 1;
            return saved?  Ok("user successfully added") : BadRequest("failed to save");
        }
        
        
        [HttpPost]
        public async Task<IActionResult> RemoveUserToRoom(string username, string roomName)
        {
            var user = await _db.Users.Include(x=> x.Rooms).FirstOrDefaultAsync(x=> x.Username == username);
            if (user == null) return NotFound("user not found");
            
            var room = await _db.Rooms.Include(x => x.Users).FirstOrDefaultAsync(x=> x.Name == roomName);
            if (room == null) return NotFound("room not found");

            if (!room.Users.Contains(user)) return Ok("user is not in room");
            
            room.Users.Remove(user);
            
            var saved = await _db.SaveChangesAsync() > 1;
            return saved?  Ok("user successfully added") : BadRequest("failed to save");
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRoomsForUser(string username)
        {
            var user = await _db.Users.Include(x=> x.Rooms).FirstOrDefaultAsync(x=> x.Username == username);
            if (user == null) return NotFound("user not found");

            return Ok(user.Rooms);
        }

        
        
}