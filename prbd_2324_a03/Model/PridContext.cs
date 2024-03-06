using Microsoft.EntityFrameworkCore;
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

        // l'entité Member ...
        modelBuilder.Entity<Users>()
            // doit utiliser la propriété Role comme discriminateur ...
            .HasDiscriminator(m => m.Role)
            // en mappant la valeur Role.Member sur le type Member ...
            .HasValue<Users>(Role.User)
            // et en mappant la valeur Role.Administator sur le type Administrator ...
            .HasValue<Administrator>(Role.Administrator);

    }


    public DbSet<Users> Users => Set<Users>();
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<Tricounts> Tricounts => Set<Tricounts>();
}