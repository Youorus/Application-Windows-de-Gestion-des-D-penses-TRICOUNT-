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
        modelBuilder.Entity<User>()
            // doit utiliser la propriété Role comme discriminateur ...
            .HasDiscriminator(m => m.Role)
            // en mappant la valeur Role.Member sur le type Member ...
            .HasValue<User>(Role.User)
            // et en mappant la valeur Role.Administator sur le type Administrator ...
            .HasValue<Administrator>(Role.Administrator);

        modelBuilder.Entity<User>()
              // doit utiliser la propriété Role comme discriminateur ...
              .HasDiscriminator(m => m.Role)
              // en mappant la valeur Role.Member sur le type Member ...
              .HasValue<User>(Role.User)
              // et en mappant la valeur Role.Administator sur le type Administrator ...
              .HasValue<Administrator>(Role.Administrator);

        modelBuilder.Entity<Tricounts>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Tricounts>()
            .HasOne(t => t.CreatorTricount)
            .WithMany(u => u.CreatedTricounts)
            .HasForeignKey(t => t.Creator)
            .OnDelete(DeleteBehavior.Restrict); // ON DELETE NO ACTION


        // Add Subscriptions entity
        modelBuilder.Entity<Subscriptions>()
            .HasKey(s => new { s.TricountId, s.UserId }) ;

        modelBuilder.Entity<Subscriptions>()
            .HasOne(s => s.Tricount)
            .WithMany(t => t.Subscriptions)
            .HasForeignKey(s => s.TricountId);


        modelBuilder.Entity<Subscriptions>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId).
            OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Operations>()
           .HasOne(o => o.Tricount)
           .WithMany(t => t.Operations )
           .HasForeignKey(o => o.TricountId).
           OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Operations>()
          .HasOne(o => o.Creator)
          .WithMany(u => u.CreatedOperations)
          .HasForeignKey(o => o.InitiatorId).
          OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Repartitions>()
           .HasKey(r => new {r.OperationId , r.UserId }); 


        modelBuilder.Entity<Repartitions>()
         .HasOne(r => r.Operations)
         .WithMany(o => o.Repartitions)
         .HasForeignKey(o => o.OperationId).
         OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Repartitions>()
        .HasOne(r => r.User)
        .WithMany(u => u.Repartitions)
        .HasForeignKey(o => o.UserId).
        OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Templates>()
      .HasOne(t => t.Tricount)
      .WithMany(t => t.Templates)
      .HasForeignKey(t => t.TricountId)
      .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Template_items>()
            .HasKey(t => new { t.UserId, t.TemplateId });

        modelBuilder.Entity<Template_items>()
            .HasOne(t => t.Templates)
            .WithMany(t => t.Template_Items)
            .HasForeignKey(t => t.TemplateId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Template_items>()
            .HasOne(t => t.User)
            .WithMany(u => u.Template_Items)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);


    }


    public DbSet<User> Users => Set<User>();
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<Tricounts> Tricounts => Set<Tricounts>();
    public DbSet<Operations> Operations => Set<Operations>();

    public DbSet<Repartitions> Repartitions => Set<Repartitions>();

    public DbSet<Templates> Templates => Set<Templates>();

    public DbSet<Template_items> template_Items => Set<Template_items>();


}