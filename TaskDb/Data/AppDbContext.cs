using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using TaskDb.Models;
namespace TaskDb.Data;

public class AppDbContext : DbContext {
   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

   }
   public DbSet<TaskItem> Tasks { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<TaskItem>().HasData(
         new TaskItem {
            Id = 1,
            Title = "Изучить APS.NET Core",
            Description = "Контроллеры, маршруты, else",
            Priority = "High",
            IsCompleted = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
         },

         new TaskItem {
            Id = 2,
            Title = "Подключить SQLite через EF Core",
            Description = "Чем гуще лес if else if else",
            Priority = "High",
            IsCompleted = false,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
         },

         new TaskItem {
            Id = 3,
            Title = "Write README",
            Description = "Описать структуру проекта",
            Priority = "Normal",
            IsCompleted = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
         }
      );
   }
}