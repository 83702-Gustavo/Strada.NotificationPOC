namespace Strada.Notification.Domain.Entities;

public class Provider : BaseEntity
{
    public string Name { get; private set; }
    public string Type { get; private set; } 
    public bool IsActive { get; private set; }
    public int Priority { get; private set; }

    public Provider(string name, string type, bool isActive, int priority, string createdBy)
        : base(createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Provider name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Provider type cannot be null or empty.");

        if (priority < 1)
            throw new ArgumentException("Priority must be greater than or equal to 1.");

        Name = name;
        Type = type;
        IsActive = isActive;
        Priority = priority;
    }

    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public void UpdatePriority(int priority)
    {
        if (priority < 1)
            throw new ArgumentException("Priority must be greater than or equal to 1.");

        Priority = priority;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Name={Name}, Type={Type}, IsActive={IsActive}, Priority={Priority}";
    }
}