
using Microsoft.AspNetCore.Mvc;
using School.Api.Models;
using SchoolApi.Models;
using SchoolApi.Repositories;
using System;
using System.Threading.Tasks;


namespace SchoolApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceRepository _repository;


        public AttendanceController(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        // POST api/attendance
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Attendance attendance)
        {
            try
            {
                var newId = await _repository.AddAsync(attendance);
                return CreatedAtAction(nameof(GetByStudent), new { studentId = attendance.StudentId }, new { attendanceId = newId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR adding attendance: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/attendance/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            try
            {
                var records = await _repository.GetByStudentAsync(studentId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR fetching attendance: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/attendance/by-class-date?classId=1&date=2025-10-01
        [HttpGet("by-class-date")]
        public async Task<IActionResult> GetByClassAndDate(int classId, DateTime? date)
        {
            try
            {
                var records = await _repository.GetByClassAndDateAsync(classId, date);
                return Ok(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR fetching attendance: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/attendance/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Attendance attendance)
        {
            if (id != attendance.AttendanceId)
                return BadRequest("AttendanceId mismatch");

            try
            {
                var updated = await _repository.UpdateAsync(attendance);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR updating attendance: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/attendance/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR deleting attendance: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}