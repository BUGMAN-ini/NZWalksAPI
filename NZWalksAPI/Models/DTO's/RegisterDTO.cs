using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO_s
{
    public class RegisterDTO
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
