using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using PRBD_Framework;
using System.Configuration;

namespace prbd_2324_a03.Model;

public class PridContext : DbContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);

        /*
         * SQLite
         */

        // var connectionString = ConfigurationManager.ConnectionStrings["SqliteConnectionString"].ConnectionString;
        // optionsBuilder.UseSqlite(connectionString);

        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=tricount")
        .LogTo(Console.WriteLine, LogLevel.Information);

        var connectionString = ConfigurationManager.ConnectionStrings["MsSqlConnectionString"].ConnectionString;
        optionsBuilder.UseSqlServer(connectionString);


    }

    private static void ConfigureOptions(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseLazyLoadingProxies()
            .LogTo(Console.WriteLine, LogLevel.Information) // permet de visualiser les requêtes SQL générées par LINQ
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors() // attention : ralentit les requêtes
            ;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Configuration des entités et des relations
        modelBuilder.Entity<User>()
                    .HasDiscriminator(m => m.Role)
                    .HasValue<User>(Role.User)
                    .HasValue<Administrator>(Role.Administrator);


        modelBuilder.Entity<Tricounts>().HasKey(t => t.Id);
        modelBuilder.Entity<Tricounts>()
         .HasMany(t => t.Operations)
         .WithOne(o => o.Tricount)
         .HasForeignKey(o => o.TricountId)
         .OnDelete(DeleteBehavior.ClientCascade);
     

        modelBuilder.Entity<Subscriptions>()
                    .HasKey(s => new { s.TricountId, s.UserId });
        modelBuilder.Entity<Subscriptions>()
                    .HasOne(s => s.Tricount)
                    .WithMany(t => t.Subscriptions)
                    .HasForeignKey(s => s.TricountId);
        modelBuilder.Entity<Subscriptions>()
                    .HasOne(s => s.User)
                    .WithMany(u => u.Subscriptions)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Operations>()
                    .HasOne(o => o.Tricount)
                    .WithMany(t => t.Operations)
                    .HasForeignKey(o => o.TricountId)
                    .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Operations>()
                    .HasOne(o => o.Creator)
                    .WithMany(u => u.CreatedOperations)
                    .HasForeignKey(o => o.InitiatorId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Repartitions>()
                    .HasKey(r => new { r.OperationId, r.UserId });
        modelBuilder.Entity<Repartitions>()
                    .HasOne(r => r.Operations)
                    .WithMany(o => o.Repartitions)
                    .HasForeignKey(o => o.OperationId)
                    .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Repartitions>()
                    .HasOne(r => r.User)
                    .WithMany(u => u.Repartitions)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);




        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, Mail = "boverhaegen@epfc.eu", Password = "3D4AEC0A9B43782133B8120B2FDD8C6104ABB513FE0CDCD0D1D4D791AA42E338:C217604FDAEA7291C7BA5D1D525815E4:100000:SHA256", Full_name = "Boris", Role = Role.User },
            new User { UserId = 2, Mail = "bepenelle@epfc.eu", Password = "9E58D87797C6795D294E6762B6C05116D075BC18445AD4078C25674809DB57EF:C91E0B85B7264877C0424D52494D6296:100000:SHA256", Full_name = "Benoît", Role = Role.User },
            new User { UserId = 3, Mail = "xapigeolet@epfc.eu", Password = "5B979AB86EC73B0996F439D0BC3947ECCFA0A41310C77533EA36CB409DBB1243:0CF43009110DE4B4AA6D4E749F622755:100000:SHA256", Full_name = "Xavier", Role = Role.User },
            new User { UserId = 4, Mail = "mamichel@epfc.eu", Password = "955F147CE3473774E35EE58F4233AA84AE9118C6ECD4699DD788B8D588238034:5514D1DD0A97E9BA7FE4C0B5A4E89351:100000:SHA256", Full_name = "Marc", Role = Role.User },
            new User { UserId = 5, Mail = "admin@epfc.eu", Password = "C9949A02A5DFBE50F1DA289DC162E3C97443AB09CE6F6EB1FD0C9D51B5241BBD:5533437973C5BC6459DB687CA5BDE76C:100000:SHA256", Full_name = "Administrator", Role = Role.Administrator }
        );

        modelBuilder.Entity<Tricounts>().HasData(
    new Tricounts { Id = 1, Title = "Gers 2023",Description= "description vide", Created_at = new DateTime(2023, 10, 10, 18, 42, 24), Creator = 1 },
    new Tricounts { Id = 2, Title = "Resto badminton", Description = "description vide", Created_at = new DateTime(2023, 10, 10, 19, 25, 10), Creator = 1 },
    new Tricounts { Id = 4, Title = "Vacances", Description = "A la mer du nord", Created_at = new DateTime(2023, 10, 10, 19, 31, 9), Creator = 1 },
    new Tricounts { Id = 5, Title = "Grosse virée", Description = "A Torremolinos", Created_at = new DateTime(2023, 8, 15, 10, 0, 0), Creator = 2 },
    new Tricounts { Id = 6, Title = "Torhout Werchter", Description = "Memorabile", Created_at = new DateTime(2023, 6, 2, 18, 30, 12), Creator = 3 }
);



        modelBuilder.Entity<Subscriptions>().HasData(
          new Subscriptions { UserId = 1, TricountId = 1 },
          new Subscriptions { UserId = 1, TricountId = 2 },
          new Subscriptions { UserId = 1, TricountId = 4 },
          new Subscriptions { UserId = 1, TricountId = 6 },
          new Subscriptions { UserId = 2, TricountId = 2 },
          new Subscriptions { UserId = 2, TricountId = 4 },
          new Subscriptions { UserId = 2, TricountId = 5 },
          new Subscriptions { UserId = 2, TricountId = 6 },
          new Subscriptions { UserId = 3, TricountId = 4 },
          new Subscriptions { UserId = 3, TricountId = 5 },
          new Subscriptions { UserId = 3, TricountId = 6 },
          new Subscriptions { UserId = 4, TricountId = 4 },
          new Subscriptions { UserId = 4, TricountId = 5 },
          new Subscriptions { UserId = 4, TricountId = 6 }
      );

        modelBuilder.Entity<Operations>().HasData(
      new Operations { Id = 1, Title = "Colruyt", TricountId = 4, Amount = 100, OperationDate = new DateTime(2023, 10, 13), InitiatorId = 2 },
      new Operations { Id = 2, Title = "Plein essence", TricountId = 4, Amount = 75, OperationDate = new DateTime(2023, 10, 13), InitiatorId = 1 },
      new Operations { Id = 3, Title = "Grosses courses LIDL", TricountId = 4, Amount = 212.47, OperationDate = new DateTime(2023, 10, 13), InitiatorId = 3 },
      new Operations { Id = 4, Title = "Apéros", TricountId = 4, Amount = 31.89745622, OperationDate = new DateTime(2023, 10, 13), InitiatorId = 1 },
      new Operations { Id = 5, Title = "Boucherie", TricountId = 4, Amount = 25.5, OperationDate = new DateTime(2023, 10, 26), InitiatorId = 2 },
      new Operations { Id = 6, Title = "Loterie", TricountId = 4, Amount = 35, OperationDate = new DateTime(2023, 10, 26), InitiatorId = 1 },
      new Operations { Id = 7, Title = "Sangria", TricountId = 5, Amount = 42, OperationDate = new DateTime(2023, 8, 16), InitiatorId = 2 },
      new Operations { Id = 8, Title = "Jet Ski", TricountId = 5, Amount = 250, OperationDate = new DateTime(2023, 8, 17), InitiatorId = 3 },
      new Operations { Id = 9, Title = "PV parking", TricountId = 5, Amount = 15.5, OperationDate = new DateTime(2023, 8, 16), InitiatorId = 3 },
      new Operations { Id = 10, Title = "Tickets", TricountId = 6, Amount = 220, OperationDate = new DateTime(2023, 6, 8), InitiatorId = 1 },
      new Operations { Id = 11, Title = "Décathlon", TricountId = 6, Amount = 199.99, OperationDate = new DateTime(2023, 7, 1), InitiatorId = 2 }
  );

        modelBuilder.Entity<Repartitions>().HasData(
    // Operation 1
    new Repartitions { OperationId = 1, UserId = 1, Weight = 1 },
    new Repartitions { OperationId = 1, UserId = 2, Weight = 1 },
    // Operation 2
    new Repartitions { OperationId = 2, UserId = 1, Weight = 1 },
    new Repartitions { OperationId = 2, UserId = 2, Weight = 1 },
    // Operation 3
    new Repartitions { OperationId = 3, UserId = 1, Weight = 2 },
    new Repartitions { OperationId = 3, UserId = 2, Weight = 1 },
    new Repartitions { OperationId = 3, UserId = 3, Weight = 1 },
    // Operation 4
    new Repartitions { OperationId = 4, UserId = 1, Weight = 1 },
    new Repartitions { OperationId = 4, UserId = 2, Weight = 2 },
    new Repartitions { OperationId = 4, UserId = 3, Weight = 3 },
    // Operation 5
    new Repartitions { OperationId = 5, UserId = 1, Weight = 2 },
    new Repartitions { OperationId = 5, UserId = 2, Weight = 1 },
    new Repartitions { OperationId = 5, UserId = 3, Weight = 1 },
    // Operation 6
    new Repartitions { OperationId = 6, UserId = 1, Weight = 1 },
    new Repartitions { OperationId = 6, UserId = 3, Weight = 1 },
    // Operation 7
    new Repartitions { OperationId = 7, UserId = 2, Weight = 1 },
    new Repartitions { OperationId = 7, UserId = 3, Weight = 2 },
    new Repartitions { OperationId = 7, UserId = 4, Weight = 3 },
    // Operation 8
    new Repartitions { OperationId = 8, UserId = 3, Weight = 2 },
    new Repartitions { OperationId = 8, UserId = 4, Weight = 1 },
    // Operation 9
    new Repartitions { OperationId = 9, UserId = 2, Weight = 1 },
    new Repartitions { OperationId = 9, UserId = 4, Weight = 5 },
    // Operation 10
    new Repartitions { OperationId = 10, UserId = 1, Weight = 1 },
    new Repartitions { OperationId = 10, UserId = 3, Weight = 1 },
    // Operation 11
    new Repartitions { OperationId = 11, UserId = 2, Weight = 2 },
    new Repartitions { OperationId = 11, UserId = 4, Weight = 2 }
);





    }


    public DbSet<User> Users => Set<User>();

  
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<Tricounts> Tricounts => Set<Tricounts>();
    public DbSet<Operations> Operations => Set<Operations>();

    public DbSet<Subscriptions> Subscriptions => Set<Subscriptions>();

    public DbSet<Repartitions> Repartitions => Set<Repartitions>();


}