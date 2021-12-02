using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;
using AutoMapper;
using StudentAdminPortal.API.DomainModels;

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
       public async Task<IActionResult> GetAllStudentsAsync()
        {
            List<DataModels.Student> students = await _studentRepository.GetStudentsAsync();
            List<DomainModels.Student> studentDto = _mapper.Map<List<DomainModels.Student>>(students);
     
            return Ok(studentDto);
        }

        [HttpGet]
        [Route("[controller]/{studentId}")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
           DomainModels.Student student = _mapper.Map<DomainModels.Student>(await _studentRepository.GetStudentAsync(studentId));
           return student == null? NotFound() : Ok(student);
        }

        [HttpPut]
        [Route("[controller]/{studentId}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await _studentRepository.HasStudentAsync(studentId))
            {
                var studentDataModel = await _studentRepository.UpdateStudent(studentId, _mapper.Map<DataModels.Student>(request));
                return  Ok(_mapper.Map<DomainModels.Student>(studentDataModel));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete]
        [Route("[controller]/{studentId}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await _studentRepository.HasStudentAsync(studentId))
            {
                var student = await _studentRepository.DeleteStudentAsync(studentId);
                return Ok(_mapper.Map<DomainModels.Student>(student));
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var student = _mapper.Map<DataModels.Student>(request);
            var newStudnet = await _studentRepository.AddStudnetAsync(student);
            return Ok(_mapper.Map<DomainModels.Student>(newStudnet));

        }
    }
}
