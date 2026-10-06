using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Repositories;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;


namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _repository;
        private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDb;Trusted_Connection=True;";

        public SubjectsController(ISubjectRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var subject = await _repository.GetByIdAsync(id);
                if (subject == null) return NotFound();
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error fetching subject: " + ex.Message);
            }
        }

        // POST: api/subjects
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Subject subject)
        {
            if (subject == null) return BadRequest("Subject is null.");

            try
            {
                await _repository.AddAsync(subject); // just await, do not assign
                return Ok(new { message = "Subject added successfully" });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Error adding subject: " + ex.Message);
            }
        }

        // PUT: api/subjects/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Subject subject)
        {
            if (subject == null || subject.SubjectId != id)
                return BadRequest("Invalid subject data.");

            try
            {
                await _repository.UpdateAsync(subject); // just await
                return Ok(new { message = "Subject updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error updating subject: " + ex.Message);
            }
        }

        // DELETE: api/subjects/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id); // just await
                return Ok(new { message = "Subject deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error deleting subject: " + ex.Message);
            }
        }

        [HttpGet]
public async Task<IActionResult> GetAll()
{
    var subjects = await _repository.GetAllAsync();
    return Ok(subjects);
}

    }
}

   
