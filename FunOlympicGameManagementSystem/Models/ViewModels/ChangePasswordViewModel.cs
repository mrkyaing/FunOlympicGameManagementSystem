using System.ComponentModel.DataAnnotations;

namespace FunOlympicGameManagementSystem.Models.ViewModels {
    public class ChangePasswordViewModel {
        [Required(AllowEmptyStrings = false, ErrorMessage = "OTP is requierd")]
        public string OTP { get; set; }
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is requierd")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Need min 6 char")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is requierd")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password should match with Password")]
        public string ConfirmPassword { get; set; }
    }
}
