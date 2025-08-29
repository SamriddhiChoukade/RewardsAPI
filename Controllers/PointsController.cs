using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RewardsAPI.Data;

namespace RewardsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // 👈 Protected
public class PointsController : ControllerBase
{
    private readonly RewardsContext _context;
    public PointsController(RewardsContext context) => _context = context;

    [HttpPost("add")]
    public async Task<IActionResult> AddPoints([FromQuery] int amount)
    {
        if (amount <= 0) return BadRequest("Amount must be positive.");
        var memberIdClaim = User.FindFirst("memberId")?.Value;
        if (memberIdClaim is null) return Unauthorized();

        int memberId = int.Parse(memberIdClaim);
        var member = await _context.Members.FindAsync(memberId);
        if (member == null || !member.IsVerified) return Forbid();

        int points = (amount / 100) * 10;
        member.Points += points;
        await _context.SaveChangesAsync();

        return Ok(new { memberId, member.Points });
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyPoints()
    {
        var memberIdClaim = User.FindFirst("memberId")?.Value;
        if (memberIdClaim is null) return Unauthorized();
        int memberId = int.Parse(memberIdClaim);

        var member = await _context.Members.FindAsync(memberId);
        if (member == null) return NotFound();

        return Ok(new { memberId, member.Points });
    }
}
