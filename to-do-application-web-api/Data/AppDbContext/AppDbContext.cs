using Microsoft.EntityFrameworkCore;

namespace to_do_application_web_api.Data.AppDbContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
