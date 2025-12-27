namespace AdsWithQuartz.Entities;

public class Ad : BaseEntity
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ActiveUntil { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? DeactivatedAt { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}