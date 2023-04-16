global using Microsoft.EntityFrameworkCore;

namespace MinimalBookApiV2
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress; Database=minimalbookbd; Trusted_Connection=True;TrustServerCertificate=true;");
        }

        public DbSet<Book> Books => Set<Book>();
    }
}
