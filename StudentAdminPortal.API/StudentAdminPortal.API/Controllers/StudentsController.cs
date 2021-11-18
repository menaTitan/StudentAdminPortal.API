using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.DataModels;
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
       public IActionResult GetAllStudents()
        {
            List<DataModels.Student> students = _studentRepository.GetStudents();
            var domainModelStudent =  _mapper.Map<List<DomainModels.Student>>(students);
            return Ok(domainModelStudent);


        }
    }
}
