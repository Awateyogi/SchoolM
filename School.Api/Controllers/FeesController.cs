using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Repositories;

namespace SchoolApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FeesController : ControllerBase
    {
        private readonly IFeeRepository _feeRepo;

        public FeesController(IFeeRepository feeRepo)
        {
            _feeRepo = feeRepo;
        }

        
        [HttpPost]
        public async Task<IActionResult> AddFee([FromBody] Fee fee)
        {
            if (fee.Amount <= 0)
                return BadRequest("Fee amount must be greater than zero");

            var id = await _feeRepo.AddAsync(fee);
            return Ok(new { feeId = id });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Fee fee)
        {
            fee.FeeId = id;
            await _feeRepo.UpdateAsync(fee);
            return NoContent();
        }

        [HttpGet("by-student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var fees = await _feeRepo.GetByStudentIdAsync(studentId);
            return Ok(fees);
        }

        [HttpGet("by-student-class")]
        public async Task<IActionResult> GetByStudentAndClass( int studentId,  int classId)
        {
            try
            {
                var fees = await _feeRepo.GetByStudentAndClassAsync(studentId, classId);
                return Ok(fees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }


}

