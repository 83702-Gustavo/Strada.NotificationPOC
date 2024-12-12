using Microsoft.EntityFrameworkCore;

namespace Strada.Notification.Infrastructure.Persistence;
public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Domain.Entities.Notification>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Recipient).IsRequired();
            entity.Property(n => n.Message).HasMaxLength(500);
            entity.Property(n => n.CreatedAt).IsRequired();
        });
    }
}