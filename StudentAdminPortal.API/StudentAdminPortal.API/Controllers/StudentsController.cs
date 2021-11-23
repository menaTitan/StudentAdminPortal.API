using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;
using AutoMapper;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public IMapper Mapper { get; }

        [HttpGet]
        [Route("[controller]")]
       public async Task<IActionResult> GetAllStudents()
        {
            List<DataModels.Student> students = await _studentRepository.GetStudentsAsync();
            List<DomainModels.Student> studentDto = _mapper.Map<List<DomainModels.Student>>(students);
     
            return Ok(studentDto);
        }

        [HttpGet]
        [Route("[controller]/{studentId}")]
        public async Task<IActionResult> GetStudent([FromRoute] Guid studentId)
        {
           DomainModels.Student student = _mapper.Map<DomainModels.Student>(await _studentRepository.GetStudentAsync(studentId));
           return student == null? NotFound() : Ok(student);
        }
    }
}
