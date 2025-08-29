namespace RewardsAPI.Models;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string OTP { get; set; } = string.Empty;
    public int Points { get; set; }
    public bool IsVerified { get; set; } = false;   // 👈 NEW
}
