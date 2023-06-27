using System.ComponentModel.DataAnnotations;

namespace FunOlympicGameManagementSystem.Models.ViewModels {
    public class UserViewModel {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Password { get; set; }
        public string Gender { get; set; }
        public string? Country { get; set; }
        public string Address { get; set; }
    }
}
