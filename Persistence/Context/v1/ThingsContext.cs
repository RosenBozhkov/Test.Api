using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Confiig;
using Persistence.Entities.Abstract;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
    public virtual DbSet<UserProfile> UserProfiles { get; set; }
   

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

    public override int SaveChanges()
    {
        //UpdateBaseEntities();
        return base.SaveChanges();
    }
    
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateBaseEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
    {
        UpdateBaseEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //UpdateBaseEntities();
        return base.SaveChangesAsync();
    }

    private void UpdateBaseEntities()
    {
        IEnumerable<EntityEntry> entities = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entities)
        {
            DateTime date = DateTime.UtcNow;
            BaseEntity baseEntity = (BaseEntity)entry.Entity;
            baseEntity.ModifiedAt = date;
            if (entry.State == EntityState.Added)
            {
                baseEntity.CreatedAt = date;
            }
        }
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}