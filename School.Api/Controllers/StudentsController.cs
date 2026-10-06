//using Microsoft.AspNetCore.Mvc;
//using SchoolApi.Models;
//using SchoolApi.Repositories;

//namespace SchoolApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StudentsController : ControllerBase
//    {
//        private readonly StudentRepository _repo;
//        public StudentsController(StudentRepository repo)
//        {
//            _repo = repo;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

//        [HttpPost]
//        public async Task<IActionResult> Add(Student student)
//        {
//            if (student == null) return BadRequest("Invalid student data");

//            var newId = await _repo.AddAsync(student);
//            student.StudentId = newId;
//            return CreatedAtAction(nameof(GetById), new { id = newId }, student);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            var student = await _repo.GetByIdAsync(id);
//            if (student == null) return NotFound();
//            return Ok(student);
//        }

//        //[HttpPost]
//        //public async Task<IActionResult> Add([FromBody] Student student)
//        //{
//        //    await _repo.AddAsync(student);
//        //    return Ok();
//        //}

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] Student student)
//        {
//            student.StudentId = id;
//            await _repo.UpdateAsync(student);
//            return Ok();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            await _repo.DeleteAsync(id);
//            return Ok();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Repositories;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IClassRepository _classRepository;
        private readonly IDivisionRepository _divisionRepository;

        public StudentsController(IStudentRepository repository, IClassRepository classRepository, IDivisionRepository divisionRepository)
        {
            _repository = repository;
            _classRepository = classRepository;
            _divisionRepository = divisionRepository;
        }
        // GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var students = await _repository.GetAllAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR fetching students: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var student = await _repository.GetByIdAsync(id); // call your repository
                if (student == null)
                    return NotFound("Student not found.");
                return Ok(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR in GetById: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/students
       
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Student student)
        {
            if (student == null)
                return BadRequest("Invalid student data");

            // ✅ Validate ClassId exists
            if (student.ClassId != null)
            {
                var classExists = await _classRepository.ExistsAsync(student.ClassId.Value);
                if (!classExists)
                    return BadRequest("Class does not exist."); // return 400 if invalid
            }


            try
            {
                var id = await _repository.AddAsync(student);
                return Ok(new { StudentId = id });
            }
            catch (Exception ex)
            {
                // log error to console
                //Console.WriteLine("ERROR in Add student: " + ex.Message);
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.Message); // send actual error to frontend
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Add([FromBody] Student student)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var newId = await _repository.AddAsync(student);
        //    student.StudentId = newId;

        //    return CreatedAtAction(nameof(GetById), new { id = newId }, student);
        //}

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Student student)
        {
            if (id != student.StudentId)
                return BadRequest("StudentId mismatch");

            var updated = await _repository.UpdateAsync(student);

            if (!updated)
                return NotFound(); // student with that ID not found

            return NoContent(); // ✅ success, no data to return
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("by-class")]
        public async Task<IActionResult> GetByClass(int classId)
        {
            var students = await _repository.GetByClassAsync(classId);
            return Ok(students);
        }

        [HttpGet("by-class-division")]
        public async Task<IActionResult> GetByClassAndDivision(int classId, int divisionId)
        {
            var students = await _repository.GetByClassAndDivisionAsync(classId, divisionId);

            if (students == null || !students.Any())
                return NotFound();

            return Ok(students);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var students = await _repository.GetAllAsync();
        //    if (students == null || !students.Any())
        //        return NotFound("No students found");

        //    return Ok(students);
        //}



    }
}
