using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _301247589_301276375_bright_aid_API.Models;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;

namespace _301247589_301276375_bright_aid_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentContext _context;
        private readonly IMapper _mapper;

        public StudentsController(StudentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(long id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Students/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStudent([FromRoute] long id, [FromBody] JsonPatchDocument<Student> patchDoc)
        {
            // If the received data is null
            if (patchDoc == null)
            {
                return BadRequest();
            }

            // Retrieve book from database
            var student = await _context.Students.SingleOrDefaultAsync(x => x.Id == id);

            // Check if is the book exist or not
            if (student == null)
            {
                return NotFound();
            }

            // Map retrieved book to BookModel with other properties (More or less with exactly same name)
            var studentToPatch = _mapper.Map<Student>(student);

            // Apply book to ModelState
            patchDoc.ApplyTo(studentToPatch, ModelState);

            // Use this method to validate your data
            TryValidateModel(studentToPatch);

            // If model is not valid, return the problem
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assign entity changes to original entity retrieved from database
            _mapper.Map(studentToPatch, student);

            // Say to entity framework that you have changes in book entity and it's modified
            _context.Entry(student).State = EntityState.Modified;

            // Save changes to database
            await _context.SaveChangesAsync();

            // If everything was ok, return no content status code to users
            return NoContent();
        }

        private bool StudentExists(long id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
