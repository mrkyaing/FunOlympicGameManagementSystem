using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunOlympicGameManagementSystem.Models {
    [Table("Users")]
    public class UserEntity:BaseEntity {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string? Country { get; set; }
        public string Address { get; set; }
        public bool IsEmailVerification { get; set; }
        public string Role { get; set; }

    }
}
