using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ef_ids_atop_seeded_ids
{
  class Program
  {
    static async Task Main(string[] args)
    {
      // Demonstrate letting the change tracker assign IDs from the get-go
      Console.WriteLine("Change tracker assigns IDs from the get-go:");

      using (var appDbContext = new AppDbContext())
      {
        await appDbContext.Database.EnsureDeletedAsync();
        await appDbContext.Database.EnsureCreatedAsync();
        var user1 = new User { Name = "First User" };
        var user2 = new User { Name = "Second User" };
        await appDbContext.Users.AddAsync(user1);
        await appDbContext.Users.AddAsync(user2);
        await appDbContext.Documents.AddAsync(new Document { User = user1, Content = "First user's document" });
        await appDbContext.Documents.AddAsync(new Document { User = user2, Content = "Second user's document" });
        await appDbContext.SaveChangesAsync();
      }

      using (var appDbContext = new AppDbContext())
      {
        foreach (var user in appDbContext.Users)
        {
          Console.WriteLine($"User ( Id = {user.Id}, Name = {user.Name} )");
        }

        foreach (var document in appDbContext.Documents)
        {
          Console.WriteLine($"Document ( Id = {document.Id}, UserId = {document.UserId}, Content = {document.Content} )");
        }
      }

      // Demonstrate seeding the database with hardcoded IDs and then letting the change tracker add more
      Console.WriteLine("Database is seeding with hardcoded IDs and then change tracker assigns IDs");

      using (var appDbContext = new AppDbContext())
      {
        await appDbContext.Database.EnsureDeletedAsync();
        await appDbContext.Database.EnsureCreatedAsync();
        await appDbContext.Users.AddAsync(new User { Id = 1, Name = "First User" });
        await appDbContext.Users.AddAsync(new User { Id = 2, Name = "Second User" });
        await appDbContext.Documents.AddAsync(new Document { Id = 1, UserId = 1, Content = "First user's document" });
        await appDbContext.Documents.AddAsync(new Document { Id = 2, UserId = 2, Content = "Second user's document" });

        // Let the change tracker figure out an ID for this one:
        await appDbContext.Users.AddAsync(new User { Name = "Third user" });

        await appDbContext.SaveChangesAsync();
      }

      using (var appDbContext = new AppDbContext())
      {
        foreach (var user in appDbContext.Users)
        {
          Console.WriteLine($"User ( Id = {user.Id}, Name = {user.Name} )");
        }

        foreach (var document in appDbContext.Documents)
        {
          Console.WriteLine($"Document ( Id = {document.Id}, UserId = {document.UserId}, Content = {document.Content} )");
        }
      }
    }
  }

  class AppDbContext : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite($"Data Source={nameof(ef_ids_atop_seeded_ids)}.db");
      optionsBuilder.EnableSensitiveDataLogging();

      var serviceCollection = new ServiceCollection();
      serviceCollection.AddLogging(builder =>
        builder
          .AddConsole()
          .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Trace)
      );

      var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
      optionsBuilder.UseLoggerFactory(loggerFactory);
    }
  }

  public class User
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }

  public class Document
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Content { get; set; }
  }
}
