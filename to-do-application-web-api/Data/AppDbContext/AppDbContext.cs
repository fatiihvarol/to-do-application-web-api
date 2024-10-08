using Microsoft.EntityFrameworkCore;
using to_do_application_web_api.Data.Entity;

namespace to_do_application_web_api.Data.AppDbContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<VpTodoItem> VpTodoItems { get; set; }
        public DbSet<VpUser> VpUsers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VpTodoItem>()
                .HasOne(x => x.User)
                .WithMany(x => x.VpTodoItems)
                .HasForeignKey(x => x.UserId);
        }
    }
}
