using System;

namespace Strada.Notification.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    public BaseEntity()
    {
        
    }

    protected BaseEntity(string createdBy)
    {
        if (string.IsNullOrWhiteSpace(createdBy))
            throw new ArgumentException("CreatedBy cannot be null or empty.");

        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}, CreatedAt={CreatedAt}, CreatedBy={CreatedBy}]";
    }
}