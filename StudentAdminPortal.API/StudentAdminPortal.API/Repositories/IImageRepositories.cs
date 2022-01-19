using System.Net;

namespace StudentAdminPortal.API.Repositories
{
    public interface IImageRepositories
    {
        public Task<string> Upload(IFormFile file, string fileName);

        public Task<IFormFile> Get(String filePath);
    }
}
