using Microsoft.EntityFrameworkCore;
using Persistence.Entities.v1;


namespace Persistence.Context.v1;

/// <summary>
/// DbContext
/// </summary>
public partial class ThingsContext : DbContext
{
    public ThingsContext()
    {
    }

    public ThingsContext(DbContextOptions<ThingsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<Model> Models { get; set; }
    public virtual DbSet<Make> Makes { get; set; }
    public virtual DbSet<Visit> Visits { get; set; }
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.\\ ;Database=testapi;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<Visit>()
            .HasMany(v => v.Jobs)
            .WithMany(s => s.Visits);

        modelBuilder.Entity<Job>()
            .HasMany(v => v.Visits)
            .WithMany(s => s.Jobs);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}