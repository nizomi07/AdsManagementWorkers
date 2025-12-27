using AdsWithQuartz.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdsWithQuartz.Data;
    
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Ad> Ads { get; set; }
    public DbSet<Category> Categories { get; set; }
}