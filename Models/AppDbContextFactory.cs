using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ChatForLife.Models
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // SQL Server bağlantı dizesi - bunu kendi projenize göre değiştirin
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ChatForLifeDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}