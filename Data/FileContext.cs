using Microsoft.EntityFrameworkCore;

namespace Bank.NET___backend.Data
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options) : base(options)
        {

        }
    }
}
