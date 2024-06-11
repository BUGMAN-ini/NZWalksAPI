using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO_s
{
    public class UploadImageDTO
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? FileDescription { get; set; }
    }
}
