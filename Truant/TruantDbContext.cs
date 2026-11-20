using Microsoft.EntityFrameworkCore;
using Truant.Entities;

namespace Truant;

public class TruantDbContext(DbContextOptions<TruantDbContext> options) : DbContext(options)
{
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<DailyStatistics> DailyStatistics { get; set; }
    public DbSet<ProfessorConfig> ProfessorConfigs { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<QuestionRecord> QuestionRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DailyStatistics>()
            .HasIndex(ds => new { ds.SemesterId, ds.Day })
            .IsUnique();

        modelBuilder.Entity<AttendanceRecord>()
            .HasIndex(ar => new { ar.SemesterId, ar.Day, ar.Subject })
            .IsUnique();

        modelBuilder.Entity<QuestionRecord>()
            .HasIndex(qr => new { qr.SemesterId, qr.Day, qr.Subject })
            .IsUnique();
    }
}