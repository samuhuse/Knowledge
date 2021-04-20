using HostedServices.Model;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HostedServices.Data
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
