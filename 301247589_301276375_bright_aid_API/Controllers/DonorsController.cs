using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class DonorsController : Controller
    {
        private readonly IDonorRepository _repository;
        private readonly IMapper _mapper;

        public DonorsController(IDonorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Donors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonorDto>>> GetDonors()
        {
            var Donors = await _repository.GetAllDonorsAsync();
            var DonorDtos = _mapper.Map<IEnumerable<DonorDto>>(Donors);
            //return Ok(Donors);
            return Ok(DonorDtos);


        }

        // GET: api/Donors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonorDto>> GetDonor(long id)
        {
            var Donor = await _repository.GetDonorByIdAsync(id);
            if (Donor == null)
            {
                return NotFound();
            }
            var DonorDto = _mapper.Map<DonorDto>(Donor);
            return Ok(DonorDto);
        }

        // PUT: api/Donors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonor(int id, [FromBody] DonorForUpdateDto DonorDto)
        {
            if (!await _repository.DonorExistsAsync(id))
            {
                return NotFound();
            }

            var Donor = _mapper.Map<Donor>(DonorDto);
            Donor.DonorId = id; // Ensure the ID remains consistent

            await _repository.UpdateDonorAsync(Donor);

            try
            {
                await _repository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.GetDonorByIdAsync(id) == null)
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
        // POST: api/Donors
        [HttpPost]
        public async Task<ActionResult<DonorDto>> PostDonor([FromBody] DonorForCreationDto DonorDto)
        {
            var Donor = _mapper.Map<Donor>(DonorDto);
            await _repository.AddDonorAsync(Donor);
            await _repository.SaveAsync();

            var createdDonor = _mapper.Map<DonorDto>(Donor);

            return CreatedAtAction(nameof(GetDonor), new { id = createdDonor.DonorId }, createdDonor);
        }

        // DELETE: api/Donors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(long id)
        {
            var Donor = await _repository.GetDonorByIdAsync(id);
            if (Donor == null)
            {
                return NotFound();
            }

            await _repository.DeleteDonorAsync(id);
            await _repository.SaveAsync();
            return NoContent();
        }

        // PATCH: api/Donors/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDonor([FromRoute] long id, [FromBody] JsonPatchDocument<Donor> patchDoc)
        {
            // If the received patch document is null
            if (patchDoc == null)
            {
                return BadRequest();
            }

            // Retrieve the Donor from the repository
            var Donor = await _repository.GetDonorByIdAsync(id);

            // Check if the Donor exists
            if (Donor == null)
            {
                return NotFound();
            }

            // Create a copy of the Donor for patching
            var DonorToPatch = _mapper.Map<Donor>(Donor);

            // Apply the patch document to the Donor copy
            patchDoc.ApplyTo(DonorToPatch, ModelState);

            // Validate the patched model
            TryValidateModel(DonorToPatch);

            // If model is not valid, return the problem
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the patched changes back to the original entity
            _mapper.Map(DonorToPatch, Donor);

            // Update the Donor entity in the repository
            await _repository.UpdateDonorAsync(Donor);

            // Save changes via the repository
            var saveResult = await _repository.SaveAsync();

            // Check if save was successful
            if (!saveResult)
            {
                return StatusCode(500, "An error occurred while saving the Donor.");
            }

            // If everything was ok, return no content status code
            return NoContent();
        }

        private async Task<bool> DonorExists(long id)
        {
            return await _repository.DonorExistsAsync(id);
        }
    }
}
