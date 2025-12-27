using System.ComponentModel.DataAnnotations;

namespace AdsWithQuartz.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}