using Microsoft.EntityFrameworkCore;

namespace StoreParser.Data
{
    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(DbContextOptions<EntityFrameworkContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Price>().ToTable("Prices"); 

            modelBuilder.Entity<Price>()
            .HasOne(s => s.Item)
            .WithMany(g => g.Prices)
            .HasForeignKey(s => s.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
