using Microsoft.EntityFrameworkCore;
using YourProject.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<CsvFile> CsvFiles { get; set; }
    public DbSet<CsvRecord> CsvRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<CsvFile>()
            .HasOne(f => f.User)
            .WithMany(u => u.CsvFiles)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CsvRecord>()
            .HasOne(r => r.CsvFile)
            .WithMany(f => f.Records)
            .HasForeignKey(r => r.FileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

