using Microsoft.EntityFrameworkCore;

namespace _301247589_301276375_bright_aid_API.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
    }
}
