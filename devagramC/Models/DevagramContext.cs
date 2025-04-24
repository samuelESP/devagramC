using Microsoft.EntityFrameworkCore;

namespace devagramC.Models
{
    public class DevagramContext : DbContext
    {
        public DevagramContext(DbContextOptions<DevagramContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        protected DevagramContext()
        {
        }
    }
}
