using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class LocalImageRepository : IFileUploadRepository
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly NZWalksDbContext _dbcontex;

        public LocalImageRepository(IWebHostEnvironment environment, IHttpContextAccessor contextAccessor, NZWalksDbContext dbcontex)
        {
            _environment = environment;
            _contextAccessor = contextAccessor;
            _dbcontex = dbcontex;
        }


        public async Task<Image> upload(Image image)
        {
            var localFilePath = Path.Combine(_environment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExstension}");


            // Upload Image To Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // localhost:1234/images/image.jpg
            
            var urlfilepath = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}{_contextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExstension}";
        
            image.FilePath = urlfilepath;


            //add image to the images table
            await _dbcontex.Images.AddAsync(image);
            await _dbcontex.SaveChangesAsync();

            return image;
        }
    }
}
