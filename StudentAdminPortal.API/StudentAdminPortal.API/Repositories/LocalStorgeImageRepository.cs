using System.Reflection;

namespace StudentAdminPortal.API.Repositories
{
    public class LocalStorgeImageRepository : IImageRepositories
    {
      public async Task<string> Upload(IFormFile file, string fileName)
      {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Images", fileName);
        using Stream fileStream = new FileStream(filePath, FileMode.Create);
        file.CopyToAsync(fileStream);

        return GetServerRelativePath(fileName);
      }

      public async Task<IFormFile> Get(string fileName)
      {
          IFormFile file = null;
          using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
          {
              TextReader tr = new StreamReader(stream);
              file.OpenReadStream();

          }
          return file;
        }

      private string GetServerRelativePath(string fileName)
      {
          return Path.Combine(@"Resources\Images", fileName);
      }
    }
}
