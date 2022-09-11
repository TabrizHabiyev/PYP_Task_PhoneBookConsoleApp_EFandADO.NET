using Microsoft.EntityFrameworkCore;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Model;


namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.DataContext;


public class PhoneBookContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=TABRIZ\\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;");
    }
    public DbSet<Person> Persons { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasKey(x => x.Id);
        modelBuilder.Entity<Person>().Property(x => x.Name).HasMaxLength(50);
        modelBuilder.Entity<Person>().Property(x => x.Surname).HasMaxLength(50);
        modelBuilder.Entity<Person>().Property(x => x.Phone).HasMaxLength(50);
        modelBuilder.Entity<Person>().Property(x => x.Mail).HasMaxLength(50);
    }
}
