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
        private  readonly IImageRepositories _imageRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepositories imageRepositories)
        {
            _studentRepository = studentRepository;
            _imageRepository = imageRepositories;
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
        [Route("[controller]/{studentId}"), ActionName("GetStudentAsync")]
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
            var newStudnet = _mapper.Map<DomainModels.Student>(await _studentRepository.AddStudentAsync(student));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id}, newStudnet);

        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var VaildExtension = new List<string>() {".jpeg", ".png", ".gif", ".jpg"};

            if (profileImage != null && profileImage.Length > 0)
            {
                if (VaildExtension.Contains(Path.GetExtension(profileImage.FileName).ToLower()))
                {
                    //check if the student exist
                    if (await _studentRepository.HasStudentAsync(studentId))
                    {
                        // Upload the profile image to local storage
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImageReltivePath = await _imageRepository.Upload(profileImage, fileName);

                        //update the profile image path in the database
                        if (await _studentRepository.UpdateProfileImageAsync(studentId, fileImageReltivePath))
                            return StatusCode(StatusCodes.Status202Accepted, "The profile picture has been Updated.");

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while uploading image.");
                    }
                }
            }


            return BadRequest("This is not a valid image response");
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}/user-images")]
        public async Task<IActionResult> GetUserProfileImage([FromRoute] Guid studentId)
        {
            //check if the student exist
            if (await _studentRepository.HasStudentAsync(studentId))
            {
                var studentImagePath =  _studentRepository.GetStudentAsync(studentId).Result.ProfileImageUrl;
                return Ok(_imageRepository.Get(studentImagePath));
            }
            return NotFound();

        }
    }

}
