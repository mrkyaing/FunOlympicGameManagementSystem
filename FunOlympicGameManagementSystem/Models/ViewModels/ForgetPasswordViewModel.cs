using System.ComponentModel.DataAnnotations;

namespace FunOlympicGameManagementSystem.Models.ViewModels {
    public class ForgetPasswordViewModel {
        [Required]
        public string EmailId { get; set; }
    }
}
