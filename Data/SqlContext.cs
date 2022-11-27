using Microsoft.EntityFrameworkCore;

namespace Bank.NET___backend.Data
{
    public class SqlContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }

        //private readonly string _connectionString;

        //public SqlContext(string password)
        //{
        //    _connectionString = $"Server=bank-project-postresql.postgres.database.azure.com;Database=bank-project-postgresql;Port=5432;User Id=bankserveradmin;Password={password};";
        //}


        public SqlContext(DbContextOptions<SqlContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //$"Server=bank-project-postresql.postgres.database.azure.com;Database=bank-project-postgresql;Port=5432;User Id=bankserveradmin;Password={System.Environment.GetEnvironmentVariable("SQL_PASSWORD")};"
            optionsBuilder.UseNpgsql($"Server=bank-project-postresql.postgres.database.azure.com;Database=bank-project-postgresql;Port=5432;User Id=bankserveradmin;Password={System.Environment.GetEnvironmentVariable("SQL_PASSWORD")};");
        }
    }
}
