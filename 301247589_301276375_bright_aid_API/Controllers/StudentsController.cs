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
using _301247589_301276375_bright_aid_API.DTOs;

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
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _repository.GetAllStudentsAsync();
            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
 
            return Ok(studentDtos);


        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(long id)
        {
            var student = await _repository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(long id, [FromBody] StudentForUpdateDto studentDto)
        {
            if (!await _repository.StudentExistsAsync(id))
            {
                return NotFound();
            }

            var student = _mapper.Map<Student>(studentDto);
            student.Id = id; // Ensure the ID remains consistent

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
        public async Task<ActionResult<StudentDto>> PostStudent([FromBody] StudentForCreationDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            await _repository.AddStudentAsync(student);
            await _repository.SaveAsync();

            var createdStudent = _mapper.Map<StudentDto>(student);

            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            var student = await _repository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

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
