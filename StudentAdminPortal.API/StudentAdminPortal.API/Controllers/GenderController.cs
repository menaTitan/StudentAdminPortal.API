using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class GenderController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public GenderController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult>GetAllGenders()
        {
            List<DataModels.Gender> gendersData = await _studentRepository.GetGendersAsync();
            List<DomainModels.Gender>  genders = _mapper.Map<List<DomainModels.Gender>>(gendersData);
            return genders == null ? NotFound() :  Ok(genders);
        }
    }
}
