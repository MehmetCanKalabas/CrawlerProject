using CrawlerProject.Models.Entity;
using CrawlerProject.Models.Service;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CrawlerProject.Models.Context
{
    public class MasterDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:ConnectionStringCrawler"]);
        }
        public DbSet<TEAM> Teams { get; set; }
        public DbSet<PLAYER> Players { get; set; }
    }
}