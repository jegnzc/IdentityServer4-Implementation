using System.Net;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PracticeNetCoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public StudentController(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Student>> Get()
        {
            var claims = User.Claims;
            var a = _repository.Student.FindAll(true).AsEnumerable();
            return Ok(a);
        }
    }
}