namespace AdsWithQuartz.DTOs;

public class UpdateAdDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }

    public DateTime ActiveUntil { get; set; }

    public int CategoryId { get; set; }
}