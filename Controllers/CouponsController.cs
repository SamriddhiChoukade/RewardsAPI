using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardsAPI.Data;
using RewardsAPI.Models;

namespace RewardsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // 👈 Protected
public class CouponsController : ControllerBase
{
    private readonly RewardsContext _context;
    public CouponsController(RewardsContext context) => _context = context;

    // POST /api/coupons/redeem?value=50  OR 100
    [HttpPost("redeem")]
    public async Task<IActionResult> Redeem([FromQuery] int value)
    {
        if (value != 50 && value != 100)
            return BadRequest("Coupon value must be 50 or 100.");

        var memberIdClaim = User.FindFirst("memberId")?.Value;
        if (memberIdClaim is null) return Unauthorized();
        int memberId = int.Parse(memberIdClaim);

        var member = await _context.Members.FindAsync(memberId);
        if (member == null || !member.IsVerified) return Forbid();

        int required = value == 100 ? 1000 : 500;
        if (member.Points < required) return BadRequest("Not enough points");

        member.Points -= required;
        var coupon = new Coupon { MemberId = memberId, CouponValue = value };
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();

        return Ok(new { memberId, couponValue = value, remainingPoints = member.Points });
    }
}
