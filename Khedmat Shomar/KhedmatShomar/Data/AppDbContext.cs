using Microsoft.EntityFrameworkCore;

namespace KhedmatShomar.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<VisitLog> VisitLogs => Set<VisitLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VisitLog>(e =>
        {
            e.Property(v => v.Path).HasMaxLength(256);
            e.Property(v => v.IpHash).HasMaxLength(64);
            e.Property(v => v.UserAgent).HasMaxLength(512);
            e.HasIndex(v => v.VisitedAtUtc);
        });
    }
}

public class VisitLog
{
    public long Id { get; set; }
    public DateTime VisitedAtUtc { get; set; }
    public string Path { get; set; } = "";
    /// <summary>SHA-256 آی‌پی — برای شمارش یکتا بدون ذخیره IP خام</summary>
    public string IpHash { get; set; } = "";
    public string UserAgent { get; set; } = "";
}
