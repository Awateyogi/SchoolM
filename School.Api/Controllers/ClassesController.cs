using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Repositories;
using System;


namespace SchoolApi.Controllers
{

    [ApiController]
    
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly  IClassRepository _classRepository;

        public ClassesController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        } 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classes = await _classRepository.GetAllAsync();
            return Ok(classes);
        }
    }

}