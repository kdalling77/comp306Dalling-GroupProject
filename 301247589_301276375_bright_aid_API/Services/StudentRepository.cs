using _301247589_301276375_bright_aid_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301247589_301276375_bright_aid_API.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(long id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
        }

        public async Task DeleteStudentAsync(long id)
        {
            var student = await GetStudentByIdAsync(id);
            if (student != null)
            {
                student.IsActive = false; // Mark as inactive
                _context.Entry(student).State = EntityState.Modified;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> StudentExistsAsync(long id)
        {
            return await _context.Students.AnyAsync(e => e.Id == id);
        }
    }
}
