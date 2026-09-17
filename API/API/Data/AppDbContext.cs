using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ProfileInfo> ProfileInfo => Set<ProfileInfo>();
    public DbSet<Releases> Releases => Set<Releases>();
    public DbSet<Discography> Discography => Set<Discography>();
    public DbSet<DemoTracks> DemoTracks => Set<DemoTracks>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProfileInfo>().HasKey(p => p.Id);
        modelBuilder.Entity<Releases>().HasKey(r => r.Id);
        modelBuilder.Entity<Discography>().HasKey(d => d.Id);
        modelBuilder.Entity<DemoTracks>().HasKey(t => t.Id);
        modelBuilder.Entity<SystemSetting>().HasKey(s => s.Key);
    }
}