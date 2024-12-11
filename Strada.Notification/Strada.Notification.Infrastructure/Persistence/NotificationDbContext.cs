using Microsoft.EntityFrameworkCore;

namespace Strada.Notification.Infrastructure.Persistence;
public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Notificacao> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Domain.Entities.Notificacao>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Destinatario).IsRequired();
            entity.Property(n => n.Mensagem).HasMaxLength(500);
            entity.Property(n => n.DataCriacao).IsRequired();
        });
    }
}