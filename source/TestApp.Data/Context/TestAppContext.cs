using Microsoft.EntityFrameworkCore;
using TestApp.Domain.Models.Entities;

namespace TestApp.Data.Context
{
    public class TestAppContext : DbContext
    {
        private string dbConnection;

        public TestAppContext(string dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ExternalClientMessage> ExternalClientMessages { get; set; }        

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(dbConnection);
    }
}
