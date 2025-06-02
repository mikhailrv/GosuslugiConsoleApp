using GosuslugiWinForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GosuslugiWinForms.Data
{
    public class GosuslugiContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Application> Applications { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<ParameterType> ParameterTypes { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        public GosuslugiContext(DbContextOptions<GosuslugiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasBaseType<Account>();
            modelBuilder.Entity<Account>().HasIndex(a => a.Login).IsUnique();

            modelBuilder.Entity<Account>()
                .Property(a => a.Role)
                .HasConversion(new EnumToStringConverter<Role>());

            modelBuilder.Entity<Models.Application>()
                .Property(a => a.Status)
                .HasConversion(new EnumToStringConverter<ApplicationStatus>());

            modelBuilder.Entity<Rule>()
                .Property(r => r.Operator)
                .HasConversion(new EnumToStringConverter<Operator>());

            modelBuilder.Entity<ParameterType>()
                .Property(pt => pt.ValueType)
                .HasConversion(new EnumToStringConverter<Models.ValueType>());

            modelBuilder.Entity<Service>()
                .Property(s => s.ModifiedById)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(false);

            modelBuilder.Entity<Service>()
                .Property(s => s.StartDate)
                .HasColumnType("timestamp with time zone"); 

            modelBuilder.Entity<Service>()
                .Property(s => s.EndDate)
                .HasColumnType("timestamp with time zone"); 
        }
    }
}
