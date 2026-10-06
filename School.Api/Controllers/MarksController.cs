using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Repositories;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarksController : ControllerBase
    {
        private readonly  IMarkRepository _repository;

        public MarksController(IMarkRepository repository)
        {
            _repository = repository;
        }
 
        // ✅ Get all marks for a student
        [HttpGet("by-student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            try
            {
                var marks = await _repository.GetByStudentIdAsync(studentId);
                return Ok(marks);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR fetching marks: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ Add a new mark record
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Marks mark)
        {
            try
            {
                if (mark.StudentId <= 0)
                    return BadRequest("Invalid StudentId");

                if (mark.SubjectId <= 0)
                    return BadRequest("Invalid SubjectId");

                await _repository.AddAsync(mark);
                return Ok("Mark added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL ERROR in Add Mark: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Marks mark)
        {
            if (id != mark.MarkId) return BadRequest();

            try
            {
                await _repository.UpdateAsync(mark);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // ✅ Delete a mark record
        [HttpDelete("{markId}")]
        public async Task<IActionResult> Delete(int markId)
        {
            var success = await _repository.DeleteAsync(markId);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
