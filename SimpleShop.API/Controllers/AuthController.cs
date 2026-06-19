using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleShop.Service;

namespace SimpleShop.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>Login with admin credentials to receive a JWT token.</summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var adminEmail = _config["AdminCredentials:Email"];
        var adminPassword = _config["AdminCredentials:Password"];
        var adminRole = _config["AdminCredentials:Role"];

        if (dto.Email != adminEmail || dto.Password != adminPassword)
            return Unauthorized(new { message = "Invalid email or password." });

        var token = GenerateToken(dto.Email, adminRole!);

        return Ok(new LoginResponseDto
        {
            Token = token,
            Email = dto.Email,
            Role = adminRole!
        });
    }

    private string GenerateToken(string email, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_SECRET")!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireHours = int.Parse(_config["Jwt:ExpiresInHours"] ?? "8");

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expireHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
