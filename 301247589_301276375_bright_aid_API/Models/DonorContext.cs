using Microsoft.EntityFrameworkCore;

namespace _301247589_301276375_bright_aid_API.Models
{
    public class DonorContext : DbContext
    {
        public DonorContext(DbContextOptions<DonorContext> options) : base(options) { }

        public DbSet<Donor> Donors { get; set; }
    }
}
