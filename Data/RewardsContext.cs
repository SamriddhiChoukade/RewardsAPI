using Microsoft.EntityFrameworkCore;
using RewardsAPI.Models;

namespace RewardsAPI.Data;

public class RewardsContext : DbContext
{
    public RewardsContext(DbContextOptions<RewardsContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
}
