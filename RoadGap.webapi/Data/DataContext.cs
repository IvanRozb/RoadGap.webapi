using Microsoft.EntityFrameworkCore;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Data;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public virtual DbSet<TaskModel> Tasks { get; set; }
    public virtual DbSet<Category> Category { get; set; }
    public virtual DbSet<Status> Status { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), 
                builder => builder.EnableRetryOnFailure()
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskModel>()
            .ToTable("Tasks")
            .HasKey(t => t.TaskId);
        modelBuilder.Entity<Category>()
            .HasKey(t => t.CategoryId);
        modelBuilder.Entity<Status>()
            .HasKey(t => t.StatusId);
    }
}