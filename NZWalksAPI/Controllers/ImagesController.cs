using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {



        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromBody] UploadImageDTO uploadImage)
        {
            ValidateFileUpload(uploadImage);

            if(ModelState.IsValid)
            {
                var imagedomainmodel = new Image
                {
                    File = uploadImage.File,
                    FileExstension = Path.GetExtension(uploadImage.File.FileName),
                    FileSizeInBytes = uploadImage.File.Length,
                    FileNAme = uploadImage.FileName,
                    FileDescription = uploadImage.FileDescription
                };
            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(UploadImageDTO request)
        {
            var allowedExtensions = new string[] { ".jpg", ".png" , ".jpeg" };

            if(!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported file extension");
            }
            if(request.File.Length > 10485760)
            {
                ModelState.AddModelError("file","file size more than 10mb, please upload smaller file size")
            }
        }
    }
}
