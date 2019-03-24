using DataAccess.EntitiesConfigurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
   public class BookStoreContext : DbContext
   {
        public DbSet<Book> Books { get; set; }

        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
