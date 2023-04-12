using Microsoft.EntityFrameworkCore;
using RoadGap.webapi.Models;
using Task = RoadGap.webapi.Models.Task;

namespace RoadGap.webapi.Data;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Task> Tasks { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), 
                builder => builder.EnableRetryOnFailure()
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>()
            .ToTable("Tasks")
            .HasKey();
        modelBuilder.Entity<Category>()
            .HasKey();
        modelBuilder.Entity<Status>()
            .HasKey();
    }
}