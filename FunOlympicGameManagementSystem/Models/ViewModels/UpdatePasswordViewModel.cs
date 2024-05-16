using System.ComponentModel.DataAnnotations;

namespace FunOlympicGameManagementSystem.Models.ViewModels
{
    public class UpdatePasswordViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "CurrentPassword is requierd")]
        public string CurrentPassword { get; set; }
        public string Email { get; set; }
       
        [Required(AllowEmptyStrings = false, ErrorMessage = "New Password is requierd")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Need min 6 char")]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is requierd")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm Password should match with New Password")]
        public string ConfirmPassword { get; set; }
    }
}
