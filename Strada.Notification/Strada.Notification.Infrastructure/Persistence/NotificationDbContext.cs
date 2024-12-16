using Microsoft.EntityFrameworkCore;
using Strada.Notification.Domain.Entities;

namespace Strada.Notification.Infrastructure.Persistence;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Domain.Entities.Notification> Notifications { get; set; }
    public DbSet<ProviderSettings> ProviderSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Domain.Entities.Notification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Recipient).IsRequired().HasMaxLength(255);
            entity.Property(n => n.Message).IsRequired();
            entity.Property(n => n.Status).IsRequired();
            entity.Property(n => n.CreatedAt).IsRequired();
            entity.Property(n => n.DeliveredAt).IsRequired(false);
            entity.Property(n => n.Type).IsRequired();
        });

        modelBuilder.Entity<ProviderSettings>(entity =>
        {
            entity.ToTable("ProviderSettings");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.ApiKey).IsRequired(false);
            entity.Property(p => p.Priority).IsRequired();
            entity.Property(p => p.IsEnabled).IsRequired();
        });
    }
}