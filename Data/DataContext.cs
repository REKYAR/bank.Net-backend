using Bank.NET___backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.NET___backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> MyProperty { get; set; }
    }
}
