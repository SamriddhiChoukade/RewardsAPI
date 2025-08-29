namespace RewardsAPI.Models;

public class Coupon
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int CouponValue { get; set; }
}
