using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RewardsAPI.Data;
using RewardsAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace RewardsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly RewardsContext _context;
    private readonly IConfiguration _config;
    public MemberController(RewardsContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public class RegisterRequest
    {
        public string MobileNumber { get; set; } = "";
        public string? Name { get; set; }
    }

    public class VerifyRequest
    {
        public string MobileNumber { get; set; } = "";
        public string OTP { get; set; } = "";
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var existing = await _context.Members.FirstOrDefaultAsync(m => m.MobileNumber == req.MobileNumber);
        var otp = "1234"; // demo OTP
        if (existing is null)
        {
            var member = new Member
            {
                Name = req.Name ?? "User",
                MobileNumber = req.MobileNumber,
                OTP = otp,
                Points = 0,
                IsVerified = false
            };
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Member registered", memberId = member.Id, otp });
        }
        else
        {
            existing.OTP = otp;
            existing.IsVerified = false;
            await _context.SaveChangesAsync();
            return Ok(new { message = "OTP re-generated", memberId = existing.Id, otp });
        }
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyRequest req)
    {
        var member = await _context.Members.FirstOrDefaultAsync(m => m.MobileNumber == req.MobileNumber);
        if (member == null) return NotFound("Member not found.");
        if (member.OTP != req.OTP) return BadRequest("Invalid OTP.");

        member.IsVerified = true;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(member);
        return Ok(new { message = "Verified", token, memberId = member.Id });
    }

    private string GenerateJwtToken(Member member)
    {
        var claims = new[]
        {
            new Claim("memberId", member.Id.ToString()),
            new Claim("mobile", member.MobileNumber)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresMinutes"] ?? "60")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
