using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Repositories;
namespace SchoolApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DivisionsController : ControllerBase
    {
        private readonly IDivisionRepository _repository;

        public DivisionsController(IDivisionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]  // GET api/divisions
        public async Task<IActionResult> GetAll()
        { 
            var divisions = await _repository.GetAllAsync();
            return Ok(divisions);
        }

        [HttpGet("by-class/{classId}")]
        public async Task<IActionResult> GetByClassId(int classId)
        {
            var divisions = await _repository.GetByClassIdAsync(classId);
            if (divisions == null || !divisions.Any())
                return NotFound();

            return Ok(divisions);
        }
    }

}


