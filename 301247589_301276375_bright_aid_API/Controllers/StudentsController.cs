using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _301247589_301276375_bright_aid_API.Models;
using _301247589_301276375_bright_aid_API.Services;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using NuGet.Protocol.Core.Types;

namespace _301247589_301276375_bright_aid_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return Ok(await _repository.GetAllStudentsAsync());
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            var student = await _repository.GetStudentByIdAsync(id);
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
                return BadRequest();
            }

            await _repository.UpdateStudentAsync(student);

            try
            {
                await _repository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.GetStudentByIdAsync(id) == null)
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
            await _repository.AddStudentAsync(student);
            await _repository.SaveAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            await _repository.DeleteStudentAsync(id);
            await _repository.SaveAsync();
            return NoContent();
        }

        // PATCH: api/Students/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStudent([FromRoute] long id, [FromBody] JsonPatchDocument<Student> patchDoc)
        {
            // If the received patch document is null
            if (patchDoc == null)
            {
                return BadRequest();
            }

            // Retrieve the student from the repository
            var student = await _repository.GetStudentByIdAsync(id);

            // Check if the student exists
            if (student == null)
            {
                return NotFound();
            }

            // Create a copy of the student for patching
            var studentToPatch = _mapper.Map<Student>(student);

            // Apply the patch document to the student copy
            patchDoc.ApplyTo(studentToPatch, ModelState);

            // Validate the patched model
            TryValidateModel(studentToPatch);

            // If model is not valid, return the problem
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the patched changes back to the original entity
            _mapper.Map(studentToPatch, student);

            // Update the student entity in the repository
            await _repository.UpdateStudentAsync(student);

            // Save changes via the repository
            var saveResult = await _repository.SaveAsync();

            // Check if save was successful
            if (!saveResult)
            {
                return StatusCode(500, "An error occurred while saving the student.");
            }

            // If everything was ok, return no content status code
            return NoContent();
        }

        private async Task<bool> StudentExists(long id)
        {
            return await _repository.StudentExistsAsync(id);
        }
    }
}
